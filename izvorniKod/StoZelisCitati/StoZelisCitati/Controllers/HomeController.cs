﻿using System.Security.Claims;
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
        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        string userType = User.FindFirstValue(ClaimTypes.Role)!;

        if (userType == UserType.Publisher && addBookRequest.Language != "Hrvatski")
            return UnprocessableEntity("Izdavač može ponuditi samo knjige na Hrvatskom jeziku.");

        using MemoryStream memoryStream = new MemoryStream();
        await addBookRequest.CoverImage.CopyToAsync(memoryStream);
        byte[] cover = memoryStream.ToArray();

        using (TransactionScope transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            int titleId = await npgsqlRepository.AddBook(addBookRequest, userId);

            await npgsqlRepository.AddBookCover(cover, addBookRequest.CoverImage.ContentType, titleId);

            transactionScope.Complete();
        }
        return View("Success", ("Naslov objavljen.", "/account/profile"));
    }

    [Authorize]
    [HttpGet("/add-offer")]
    public async Task<IActionResult> AddOffer()
    {
        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        IEnumerable<Book> books = await npgsqlRepository.GetBooksBelongingToUser(userId);
        return View(books);
    }

    [Authorize]
    [HttpPost("/add-offer")]
    public async Task<IActionResult> AddOffer(AddOfferRequest addOfferRequest)
    {
        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        if (userId != await npgsqlRepository.GetIdOfUserThatOwnsBook(addOfferRequest.TitleId))
            return Forbid($"Korisnik nije autoriziran objaviti ponudu za naslov {addOfferRequest.TitleId}.");
        
        await npgsqlRepository.AddOffer(addOfferRequest);

        return View("Success", ("Ponuda objavljena.", "/account/profile"));
    }
}