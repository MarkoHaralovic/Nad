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
    public async Task<IActionResult> Index()
    {
        if (User.Identity?.IsAuthenticated != true)
            return View(false);
        
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId))
            return View(false);
        
        return View(await npgsqlRepository.GetNumberOfBooksBelongingToUser(userId) != 0);
    }

    [HttpGet("/markers")]
    public async Task<ActionResult<IEnumerable<MapMarker>>> Markers()
    {
        IEnumerable<User> users = await npgsqlRepository.GetUsers(true);
        
        return Ok(users.Select(x => new MapMarker(x.DisplayName, x.Latitude, x.Longitude)).ToList());
    }

    [HttpGet("/filter")]
    public async Task<IActionResult> Filter(BookQuery bookQuery)
    {
        (IEnumerable<Book> books, int pageCount) = await npgsqlRepository.FilterBooks(bookQuery);

        if (HttpContext.Request.PartialHtmx())
            return View("FilterPartial", (books, pageCount));
        
        return View((bookQuery, books, pageCount));
    }

    [HttpGet("/image/{bookId:int}")]
    public async Task<IActionResult> Image(int bookId)
    {
        BookCover cover = await npgsqlRepository.GetBookCoverForBook(bookId);
        return File(cover.Image, cover.ImageType);
    }

    [HttpGet("/book/{bookId:int}")]
    public async Task<IActionResult> Book(int bookId)
    {
        Book book = await npgsqlRepository.GetBookWithId(bookId);
        
        return View(book);
    }

    [Authorize]
    [HttpGet("/add-title")]
    public IActionResult AddBook() => View();

    [Authorize]
    [HttpPost("/add-title")]
    public async Task<IActionResult> AddBook(AddBookRequest addBookRequest)
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId))
            return Ok("ID korisnika nije validan.");

        string? userType = User.FindFirstValue(ClaimTypes.Role);
        if (userType == null)
            return Ok("Nije poznata vrsta korisnika.");

        if (userType == UserType.Publisher && addBookRequest.Language != "Hrvatski")
            return Ok("Izdavač može ponuditi samo knjige na Hrvatskom jeziku.");
        
        using MemoryStream memoryStream = new MemoryStream();
        await addBookRequest.CoverImage.CopyToAsync(memoryStream);
        byte[] cover = memoryStream.ToArray();

        using (TransactionScope transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            int titleId = await npgsqlRepository.AddBook(addBookRequest, userId);

            await npgsqlRepository.AddBookCover(cover, addBookRequest.CoverImage.ContentType, titleId);
            
            transactionScope.Complete();
        }
        HttpContext.Response.Headers["HX-Redirect"] = "/success";
        return Ok("Uspješno dodan naslov.");
    }

    [Authorize]
    [HttpGet("/add-offer")]
    public async Task<IActionResult> AddOffer()
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId))
            return Unauthorized("Logged in user has invalid id.");

        IEnumerable<Book> books = await npgsqlRepository.GetBooksBelongingToUser(userId);
        return View(books);
    }

    [Authorize]
    [HttpPost("/add-offer")]
    public async Task<IActionResult> AddOffer(AddOfferRequest addOfferRequest)
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId))
            return Unauthorized("Logged in user has invalid id.");

        if (userId != await npgsqlRepository.GetIdOfUserThatOwnsBook(addOfferRequest.TitleId))
            return Unauthorized($"Logged in user is not authorized to add offers to title with id {addOfferRequest.TitleId}");

        await npgsqlRepository.AddOffer(addOfferRequest);
        
        return Redirect("/");
    }

    [HttpGet("/success")]
    public IActionResult TransactionSuccessful() => View();
}