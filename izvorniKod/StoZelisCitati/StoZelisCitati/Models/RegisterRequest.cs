namespace StoZelisCitati.Models;

public record RegisterRequest(
    string Username,
    string Password,
    string DisplayName,
    string UserType,
    string Email,
    string PhoneNumber,
    string Address,
    string City,
    string Country);