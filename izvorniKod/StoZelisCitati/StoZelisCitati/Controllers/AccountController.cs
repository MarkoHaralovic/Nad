using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Geocoding;
using Geocoding.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoZelisCitati.Helpers;
using StoZelisCitati.Misc;
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
    public async Task<IActionResult> Login()
    {
        return View();
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([Required] string username, [Required] string password)
    {
        Models.User? user = await npgsqlRepository.GetUser(username);
        if (user == null)
            return Ok("Nepostojeće korisničko ime.");
        
        if (!user.Approved)
            return Ok("Registracija još nije odobrena.");

        if (password != user.Password)
            return Ok("Pogrešna lozinka.");
        
        var identity = new ClaimsIdentity(new List<Claim>
        {
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
    public async Task<IActionResult> Register()
    {
        return View();
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([Required] string username, [Required] string password,
        [Required] string displayName, [Required] string userType, [Required] string email,
        [Required] string phoneNumber, [Required] string address, [Required] string city, [Required] string country)
    {
        User? user = await npgsqlRepository.GetUser(username);
        if (user != null)
            return Ok("Korisničko ime je zauzeto.");
        
        if (userType != UserHelper.Antiquarian && userType != UserHelper.Middleman && userType != UserHelper.Publisher)
            return Ok("Izaberite jednu od punuđenih kategorija.");

        IGeocoder geocoder = new GoogleGeocoder {ApiKey = "AIzaSyDhmDNo6RQm3LO4JG_mjYWQFYkJhQjfgNY"};
        Address a = (await geocoder.GeocodeAsync(address, city, "", "", country)).First();
        
        await npgsqlRepository.AddUser(username, password, displayName, userType,
            email, phoneNumber, address, city, country, false, a.Coordinates.Latitude, a.Coordinates.Longitude);
        
        HttpContext.Response.Headers["HX-Redirect"] = "/account/registered";
        return Ok("Uspješna registracija");
    }
    
    [HttpGet("registered")]
    public async Task<IActionResult> Registered()
    {
        return View();
    }
    
    [Authorize(Roles = UserHelper.Admin)]
    [HttpGet("requests")]
    public async Task<IActionResult> RegistrationRequests()
    {
        IEnumerable<Models.User> unapprovedUsers = await npgsqlRepository.GetUnapprovedUsers();
        
        return View(unapprovedUsers.ToList());
    }
    
    [Authorize(Roles = UserHelper.Admin)]
    [HttpDelete("{userId:int}")]
    public async Task<IActionResult> DeleteUser(int userId)
    {
        await npgsqlRepository.DeleteUser(userId);
        return Ok();
    }
    
    [Authorize(Roles = UserHelper.Admin)]
    [HttpPut("approve/{userId:int}")]
    public async Task<IActionResult> ApproveUser(int userId)
    {
        await npgsqlRepository.ApproveUser(userId);
        return Ok();
    }
}