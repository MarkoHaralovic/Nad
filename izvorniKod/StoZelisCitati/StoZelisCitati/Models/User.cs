using System.ComponentModel.DataAnnotations;
using MvcTest.Models.Dto;

namespace MvcTest.Models;

public class User
{
    public required int Id { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required string DisplayName { get; set; }
    public required UserType UserType { get; set; }
    public required string Email { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Address { get; set; }
    public required bool Approved { get; set; }

    public string DisplayUserType => UserType switch
    {
        UserType.Administrator => "administrator",
        UserType.Antiquarian => "antikvarijat",
        UserType.Middleman => "preprodavač",
        UserType.Publisher => "izdavač",
        _ => throw new ArgumentOutOfRangeException(nameof(UserType)),
    };
}