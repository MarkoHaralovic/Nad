namespace StoZelisCitati.Models;

public record AddBookRequest(
    string Title,
    string Author,
    string Isbn,
    string Publisher,
    string TypeOfPublisher,
    string Genre,
    int Year,
    int Edition,
    string Description,
    string Language,
    string Availability,
    IFormFile CoverImage);