using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text.Encodings.Web;
using Geocoding;
using Geocoding.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoZelisCitati.Helpers;
using StoZelisCitati.Misc;
using StoZelisCitati.Models;
using User = StoZelisCitati.Models.User;

namespace StoZelisCitati.Controllers;

[Route("account")]
public class AccountController : Controller
{
    private readonly NpgsqlRepository npgsqlRepository;

    public AccountController(NpgsqlRepository npgsqlRepository)
    {
        this.npgsqlRepository = npgsqlRepository;
    }

    [HttpGet("login")]
    public IActionResult Login() => View();

    [HttpPost("login")]
    public async Task<IActionResult> Login([Required] string username, [Required] string password)
    {
        User? user = await npgsqlRepository.GetUser(username);
        if (user == null)
            return NotFound("Nepostojeće korisničko ime.");

        if (!user.Approved)
            return Conflict("Registracija još nije odobrena.");

        if (password != user.Password)
            return Conflict("Pogrešna lozinka.");

        var identity = new ClaimsIdentity(new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, username),
            new(ClaimTypes.Role, user.UserType)
        }, "Cookies");
        await HttpContext.SignInAsync(new ClaimsPrincipal(identity));

        return View("Success", ("Login uspješan.", "/"));
    }

    [HttpGet("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return Redirect("/");
    }

    [HttpGet("register")]
    public IActionResult Register() => View();

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest registerRequest)
    {
        User? user = await npgsqlRepository.GetUser(registerRequest.Username);
        if (user != null)
            return Conflict("Korisničko ime je zauzeto.");

        if (registerRequest.UserType != UserType.Antiquarian &&
            registerRequest.UserType != UserType.Middleman &&
            registerRequest.UserType != UserType.Publisher)
        {
            return BadRequest("Izaberite jednu od punuđenih kategorija.");
        }

        GoogleGeocoder geocoder = new GoogleGeocoder {ApiKey = "AIzaSyDhmDNo6RQm3LO4JG_mjYWQFYkJhQjfgNY"};

        List<GoogleAddress> addresses =
            (await geocoder.GeocodeAsync($"{registerRequest.Address} {registerRequest.City} {registerRequest.Country}"))
            .ToList();

        if (!addresses.Any())
            return Ok("Adresa nije pronađena.");

        GoogleAddress a = addresses.First();

        registerRequest.Country = a[GoogleAddressType.Country]!.ShortName;

        if (registerRequest.UserType == UserType.Antiquarian && registerRequest.Country != Country.Croatia)
            return UnprocessableEntity("Antikvarijat mora biti u Hrvatskoj.");

        if (registerRequest.UserType == UserType.Middleman && !Country.SerboCroatian.Contains(registerRequest.Country))
            return UnprocessableEntity("Preprodavač mora biti u Hrvatskoj ili u srodnim zemljama");

        await npgsqlRepository.AddUser(registerRequest, a.Coordinates.Latitude, a.Coordinates.Longitude);

        return View("Success", ("Vaš zahtjev za registraciju će pregledati administrator.", "/"));
    }

    [Authorize]
    [HttpGet("profile")]
    public async Task<IActionResult> UserProfile()
    {
        string userName = User.FindFirstValue(ClaimTypes.Name)!;

        User? user = await npgsqlRepository.GetUser(userName);
        if (user == null)
            return NotFound("User does not exist.");

        IEnumerable<Book> books = await npgsqlRepository.GetBooksBelongingToUser(user.Id);
        
        return View((user, books));
    }

    [Authorize(Roles = UserType.Admin)]
    [HttpGet("requests")]
    public async Task<IActionResult> RegistrationRequests() => View(await npgsqlRepository.GetUsers(false));

    [Authorize(Roles = UserType.Admin)]
    [HttpDelete("{userId:int}")]
    public async Task<IActionResult> DeleteUser(int userId)
    {
        await npgsqlRepository.DeleteUser(userId);
        return Ok();
    }

    [Authorize(Roles = UserType.Admin)]
    [HttpPut("approve/{userId:int}")]
    public async Task<IActionResult> ApproveUser(int userId)
    {
        await npgsqlRepository.ApproveUser(userId);
        return Ok();
    }
}