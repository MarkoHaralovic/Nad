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
    public async Task<IActionResult> Index() => View();

    [HttpGet("/markers")]
    public async Task<ActionResult<IEnumerable<MapMarker>>> Markers() => Ok(await npgsqlRepository.GetMapMarkers());

    [HttpGet("/filter")]
    public async Task<IActionResult> Filter(BookQuery bookQuery)
    {
        (IEnumerable<BookListElementRecord> books, int pageCount) = await npgsqlRepository.FilterBooks(bookQuery);

        if (HttpContext.Request.PartialHtmx())
            return View("FilterPartial", (books, pageCount));
        
        return View((bookQuery, books, pageCount));
    }
    
    
}