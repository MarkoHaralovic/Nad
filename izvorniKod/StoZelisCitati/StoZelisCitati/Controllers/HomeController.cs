using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MvcTest.Misc;
using MvcTest.Models;

namespace MvcTest.Controllers;

public class HomeController : Controller
{
    private NpgsqlRepository npgsqlRepository;

    public HomeController(NpgsqlRepository npgsqlRepository)
    {
        this.npgsqlRepository = npgsqlRepository;
    }

    //Should have log in and register buttons.
    //Once the user is logged in, the buttons disappear.
    //For the admin, a new button appears instead.
    //It leads to the registration requests page.
    public async Task<IActionResult> Index()
    {
        return View();
    }
}