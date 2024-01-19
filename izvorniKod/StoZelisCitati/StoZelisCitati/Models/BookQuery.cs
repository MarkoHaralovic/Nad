namespace StoZelisCitati.Models;

public record BookQuery(
    string? Title,
    int? YearFrom,
    int? YearTo,
    string? Author,
    string? Publisher,
    int? Edition,
    string? TypeOfPublisher,
    string? Genre,
    string? Isbn,
    string? Language,
    string? Availability,
    string? State,
    int? PriceFrom,
    int? PriceTo,
    string? Seller,
    int Page = 1);