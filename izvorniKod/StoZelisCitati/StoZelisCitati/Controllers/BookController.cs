using System.Security.Claims;
using System.Transactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoZelisCitati.Helpers;
using StoZelisCitati.Misc;
using StoZelisCitati.Models;

namespace StoZelisCitati.Controllers;

[Route("book")]
public class BookController : Controller
{
    private readonly NpgsqlRepository npgsqlRepository;

    public BookController(NpgsqlRepository npgsqlRepository)
    {
        this.npgsqlRepository = npgsqlRepository;
    }
    
    [HttpGet("{bookId:int}/image")]
    public async Task<IActionResult> Image(int bookId)
    {
        BookCover? cover = await npgsqlRepository.GetBookCoverForBook(bookId);
        if (cover == null)
            return NotFound("Korice za knjigu ne postoje.");
        
        return File(cover.Image, cover.ImageType);
    }

    [HttpGet("{bookId:int}")]
    public async Task<IActionResult> Book(int bookId)
    {
        Book? book = await npgsqlRepository.GetBookWithId(bookId);
        if (book == null)
            return NotFound("Knjiga ne postoji.");
        
        IEnumerable<Offer> offers = await npgsqlRepository.GetOffers(bookId);

        bool editPermissions = User.TryGetId(out int? id) && id == book.UserId;
        
        return View((book, offers, editPermissions));
    }

    [Authorize]
    [HttpGet("offer/{offerId:int}")]
    public async Task<IActionResult> GetOffer(int offerId)
    {
        Offer? offer = await npgsqlRepository.GetOffer(offerId);
        if (offer == null)
            return NotFound("Ponuda nije pronađena.");

        await npgsqlRepository.GetIdOfUserThatOwnsBook(offer.BookId);
        
        bool editPermissions = User.Id() == await npgsqlRepository.GetOwnerOfOffer(offerId);
        
        return View("Offer", (offer, editPermissions));  
    }

    [Authorize]
    [HttpPut("offer/{offerId:int}")]
    public async Task<IActionResult> PutOffer(int offerId, double price, string state, int count)
    {
        int? ownerId = await npgsqlRepository.GetOwnerOfOffer(offerId);
        if (ownerId == null)
            return NotFound($"Ponuda sa id {offerId} nije pronađena.");
        
        if (User.Id() != ownerId)
            return Forbid("Korisnik nemože uređivati ovu ponudu.");
        
        await npgsqlRepository.UpdateOffer(offerId, price, state, count);

        return View("Offer", (await npgsqlRepository.GetOffer(offerId), true));
    }

    [Authorize]
    [HttpDelete("offer/{offerId:int}")]
    public async Task<IActionResult> DeleteOffer(int offerId)
    {
        int? ownerId = await npgsqlRepository.GetOwnerOfOffer(offerId);
        if (ownerId == null)
            return NotFound($"Ponuda sa id {offerId} nije pronađena.");

        if (User.Id() != ownerId)
            return Forbid("Korisnik nemože uređivati ovu ponudu.");

        await npgsqlRepository.DeleteOffer(offerId);
        
        return Ok();
    }

    [Authorize]
    [HttpGet("offer/{offerId:int}/edit")]
    public async Task<IActionResult> EditOffer(int offerId)
    {
        Offer? offer = await npgsqlRepository.GetOffer(offerId);
        if (offer == null)
            return NotFound("Ponuda nije pronađena.");
        
        return View(offer);
    }
}