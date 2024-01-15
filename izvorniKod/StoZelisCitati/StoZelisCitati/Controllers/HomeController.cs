using System.Diagnostics;
using Microsoft.AspNetCore.Authentication;
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
        return View();
    }
    
    
    
    [HttpGet("/filter")]
    public async Task<IActionResult> Filter(BookQuery bookQuery)
    {
        var filter = await npgsqlRepository.FilterBooks(bookQuery);
        
        return HttpContext.Request.PartialHtmx() ? View("FilterPartial", filter) : View(filter);
    }
    
    [HttpGet("/genres")]
    public async Task<IActionResult> Genres()
    {
        List<string> genres = new List<string>
        {
            "komedija",
            "akcija",
            "drama",
            "sci-fi",
            "avantura"
        };

        return View(genres);
    }

    [HttpGet("/genre/{genre}")]
    public async Task<IActionResult> Genre(string genre)
    {
         return View("Books", await npgsqlRepository.GetBooksWithGenre(genre));
    }
}