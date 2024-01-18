namespace StoZelisCitati.Models;

public record BookQuery(
    string? Title,
    int? YearFrom,
    int? YearTo,
    string? Author = null,
    string? Publisher = null,
    int? Edition = null,
    string? TypeOfPublisher = null,
    string? Genre = null,
    string? Isbn = null,
    string? Language = null,
    string? Availability = null,
    string? State = null,
    int? PriceFrom = null,
    int? PriceTo = null,
    string? Seller = null,
    int Page = 1);