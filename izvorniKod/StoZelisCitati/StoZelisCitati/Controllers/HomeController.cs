using System.Diagnostics;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using StoZelisCitati.Models;
using StoZelisCitati.Misc;

namespace StoZelisCitati.Controllers;

public class HomeController : Controller
{
    private NpgsqlRepository npgsqlRepository;

    public HomeController(NpgsqlRepository npgsqlRepository)
    {
        this.npgsqlRepository = npgsqlRepository;
    }
    
    public async Task<IActionResult> Index()
    {
        HttpContext.User.IsInRole("admin");
        return View();
    }

    [HttpGet("/genres")]
    public async Task<IActionResult> Genres()
    {
        List<string> genres = new List<string>()
        {
            "komedija",
            "akcija",
            "drama",
            "sci-fi",
            "avantura"
        };

        return View(genres);
    }

    [HttpGet("genre/{genre}")]
    public async Task<IActionResult> Genre(string genre)
    {
         List<Book> books = (await npgsqlRepository.GetBooksWithGenre(genre)).ToList();

         return View("Books", books);
    }
}