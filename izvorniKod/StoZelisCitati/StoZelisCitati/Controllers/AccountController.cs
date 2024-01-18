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
            return Ok("Nepostojeće korisničko ime.");
        
        if (!user.Approved)
            return Ok("Registracija još nije odobrena.");

        if (password != user.Password)
            return Ok("Pogrešna lozinka.");
        
        var identity = new ClaimsIdentity(new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, username),
            new(ClaimTypes.Role, user.UserType)
        }, "Cookies");
        await HttpContext.SignInAsync(new ClaimsPrincipal(identity));

        HttpContext.Response.Headers["HX-Redirect"] = "/";
        return Ok("Uspješna prijava.");
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
            return Ok("Korisničko ime je zauzeto.");
        
        if (registerRequest.UserType != UserType.Antiquarian &&
            registerRequest.UserType != UserType.Middleman &&
            registerRequest.UserType != UserType.Publisher)
        {
            return Ok("Izaberite jednu od punuđenih kategorija.");
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
            return Ok("Antikvarijat mora biti u Hrvatskoj.");

        if (registerRequest.UserType == UserType.Middleman && !Country.SerboCroatian.Contains(registerRequest.Country))
            return Ok("Preprodavač mora biti u Hrvatskoj ili u srodnim zemljama");

        await npgsqlRepository.AddUser(registerRequest, a.Coordinates.Latitude, a.Coordinates.Longitude);
        
        HttpContext.Response.Headers["HX-Redirect"] = "/success";
        return Ok("Uspješna registracija");
    }

    [Authorize]
    [HttpGet("profile")]
    public async Task<IActionResult> UserProfile()
    {
        string? userName = User.FindFirstValue(ClaimTypes.Name);
        if (userName == null)
            return NotFound("User has no username claim.");
        
        User? user = await npgsqlRepository.GetUser(userName);
        if (user == null)
            return NotFound("User does not exist.");
        
        return View(user);
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