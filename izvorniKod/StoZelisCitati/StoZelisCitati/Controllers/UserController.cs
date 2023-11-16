using Microsoft.AspNetCore.Mvc;
using MvcTest.Misc;
using MvcTest.Models;

namespace MvcTest.Controllers;

public class UserController : Controller
{
    private readonly NpgsqlRepository npgsqlRepository;

    public UserController(NpgsqlRepository npgsqlRepository)
    {
        this.npgsqlRepository = npgsqlRepository;
    }
    
    //User registration page.
    //Leads to registration message page once the request is submitted.
    [HttpGet("/register")]
    public async Task<IActionResult> Register()
    {
        return View();
    }
    
    //User log in page.
    //Leads to home page once logged in.
    [HttpGet("/login")]
    public async Task<IActionResult> Login()
    {
        return View();
    }


    //Shows registration requests to the admin.
    [HttpGet("/registration-requests")]
    public async Task<IActionResult> RegistrationRequests()
    {
        IEnumerable<User> unapprovedUsers = await npgsqlRepository.GetUnapprovedUsers();
        
        return View(unapprovedUsers.ToList());
    }

    //Shows message to the user that their request will be reviewed.
    [HttpPost("/user/register")]
    public async Task<IActionResult> UserRegistration()
    {
        return View();
    }
    
    [HttpPost("/user/{userId:int}")]
    public async Task<IActionResult> AddUser(int userId)
    {
        // await npgsqlRepository.AddUser();
        return Ok();
    }
    
    [HttpDelete("/user/{userId:int}")]
    public async Task<IActionResult> DeleteUser(int userId)
    {
        await npgsqlRepository.DeleteUser(userId);
        return Ok();
    }
    
    [HttpPut("/user/approve/{userId:int}")]
    public async Task<IActionResult> ApproveUser(int userId)
    {
        await npgsqlRepository.ApproveUser(userId);
        return Ok();
    }
}