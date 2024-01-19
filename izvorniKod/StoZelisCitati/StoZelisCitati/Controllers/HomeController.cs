using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Transactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoZelisCitati.Helpers;
using StoZelisCitati.Models;
using StoZelisCitati.Misc;
using StoZelisCitati.Models.Dto;

namespace StoZelisCitati.Controllers;

public class HomeController : Controller
{
    private readonly NpgsqlRepository npgsqlRepository;

    public HomeController(NpgsqlRepository npgsqlRepository)
    {
        this.npgsqlRepository = npgsqlRepository;
    }

    public IActionResult Index() => View();

    [HttpGet("/markers")]
    public async Task<ActionResult<IEnumerable<MapMarker>>> Markers()
    {
        IEnumerable<User> users = await npgsqlRepository.GetUsers(true);

        return Ok(users.Select(x => new MapMarker(x.DisplayName, x.Latitude, x.Longitude)).ToList());
    }

    [HttpGet("/filter")]
    public async Task<IActionResult> Filter(BookQuery bookQuery)
    {
        (IEnumerable<(Book, Offer)> books, int pageCount) = await npgsqlRepository.FilterBooks(bookQuery);

        if (HttpContext.Request.PartialHtmx())
            return View("FilterPartial", (books, pageCount));

        return View((bookQuery, books, pageCount));
    }

    [Authorize]
    [HttpGet("/add-title")]
    public IActionResult AddBook() => View();

    [Authorize]
    [HttpPost("/add-title")]
    public async Task<IActionResult> AddBook(AddBookRequest addBookRequest)
    {
        if (User.IsInRole(UserType.Publisher) && !Language.CroatianAliases.Contains(addBookRequest.Language))
            return UnprocessableEntity("Izdavač može ponuditi samo knjige na Hrvatskom jeziku.");

        using MemoryStream memoryStream = new MemoryStream();
        await addBookRequest.CoverImage.CopyToAsync(memoryStream);
        byte[] cover = memoryStream.ToArray();

        using (TransactionScope transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            int titleId = await npgsqlRepository.AddBook(addBookRequest, User.Id());

            await npgsqlRepository.AddBookCover(cover, addBookRequest.CoverImage.ContentType, titleId);

            transactionScope.Complete();
        }
        return View("Success", ("Naslov objavljen.", "/account/profile"));
    }

    [Authorize]
    [HttpGet("/add-offer")]
    public async Task<IActionResult> AddOffer()
    {
        IEnumerable<Book> books = await npgsqlRepository.GetBooksBelongingToUser(User.Id());
        return View(books);
    }

    [Authorize]
    [HttpPost("/add-offer")]
    public async Task<IActionResult> AddOffer(AddOfferRequest addOfferRequest)
    {
        int? ownerId = await npgsqlRepository.GetIdOfUserThatOwnsBook(addOfferRequest.TitleId);
        if (ownerId == null)
            return NotFound("Book does not exist.");
        
        if (User.Id() != ownerId)
            return Forbid($"Korisnik nije autoriziran objaviti ponudu za naslov {addOfferRequest.TitleId}.");
        
        await npgsqlRepository.AddOffer(addOfferRequest);

        return View("Success", ("Ponuda objavljena.", "/account/profile"));
    }

    [HttpGet("/translation-form/{bookId:int}")]
    public async Task<IActionResult> TranslationForm(int bookId)
    {
        Book? book = await npgsqlRepository.GetBookWithId(bookId);
        if (book == null)
            return Ok("Knjiga ne postoji.");
        
        return View((bookId, await npgsqlRepository.GetUsers(UserType.Publisher)));
    }

    [HttpPost("translation-request")]
    public async Task<IActionResult> RequestTranslation([Required] int userId, [Required] int bookId)
    {
        User? user = await npgsqlRepository.GetUser(userId);
        if (user == null)
            return Ok("Korisnik nije pronađen.");

        if (user.UserType != UserType.Publisher)
            return Ok("Korisnik nije izdavač.");
        
        await npgsqlRepository.AddTranslationRequest(userId, bookId);
        return Ok("Zahtjev za prijevodom zabilježen.");
    }
}