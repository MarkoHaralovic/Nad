namespace StoZelisCitati.Models;

public record BookQuery(
    string? Title = null,
    int? YearFrom = null,
    int? YearTo = null,
    string? Author = null,
    string? Publisher = null,
    int? Edition = null,
    string? TypeOfPublisher = null,
    string? Genre = null,
    string? Isbn = null,
    string? Language = null,
    string? Availability = null,
    string? State = null,
    string? Seller = null,
    int Page = 1);