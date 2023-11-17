using System.ComponentModel.DataAnnotations;
using StoZelisCitati.Models.Dto;

namespace StoZelisCitati.Models;

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
    public required string City { get; set; }
    public required string Country { get; set; }
    public required bool Approved { get; set; }

    public string DisplayUserType => UserType switch
    {
        UserType.Admin => "admin",
        UserType.Antiquarian => "antikvarijat",
        UserType.Middleman => "preprodavač",
        UserType.Publisher => "izdavač",
        _ => throw new ArgumentOutOfRangeException(nameof(UserType)),
    };
}