using System.ComponentModel.DataAnnotations;
using Dapper;
using StoZelisCitati.Models.Dto;

namespace StoZelisCitati.Models;

public record User(
    int Id,
    string Username,
    string Password,
    string DisplayName,
    string UserType,
    string Email,
    string PhoneNumber,
    string Address,
    string City,
    string Country,
    bool Approved,
    double Latitude,
    double Longitude);