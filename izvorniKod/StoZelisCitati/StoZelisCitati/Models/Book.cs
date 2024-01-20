namespace StoZelisCitati.Models;

public record Book(
    int Id,
    string Title,
    string Author,
    string Isbn,
    int YearOfPublishing,
    string Publisher,
    string TypeOfPublisher,
    int Edition,
    string Description,
    string Language,
    string Availability,
    int UserId,
    string Genre);