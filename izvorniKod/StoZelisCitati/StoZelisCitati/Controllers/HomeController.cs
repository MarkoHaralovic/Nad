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
}