namespace StoZelisCitati.Models;

public class Book
{
    public required int Id { get; set; }
    public required string Title { get; set; }
    public required string Author { get; set; }
    public required string Isbn { get; set; }
    public required int YearOfPublishing { get; set; }
    public required string Publisher { get; set; }
    public required string TypeOfPublisher { get; set; }
    public required int Edition { get; set; }
    public required string Description { get; set; }
    public required string Language { get; set; }
    public required string Availability { get; set; }
    public required int UserId { get; set; }
    public required string Genre { get; set; }
}