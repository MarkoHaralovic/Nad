namespace StoZelisCitati.Models.Dto;

public class BookRecord
{
    public required int id_knjiga { get; set; }
    public required string naslov { get; set; }
    public required string autor { get; set; }
    public required string isbn { get; set; }
    public required int godina_izdavanja { get; set; }
    public required string izdavac { get; set; }
    public required string kategorija_izdavaca { get; set; }
    public required int broj_izdanja { get; set; }
    public required string opis { get; set; }
    public required string jezik { get; set; }
    public required string dostupnost { get; set; }
    public required int id_korisnik { get; set; }
    public required string zanr { get; set; }

    public Book ToDomainObject()
    {
        return new Book
        {
            Id = id_knjiga,
            Title = naslov,
            Author = autor,
            Isbn = isbn,
            YearOfPublishing = godina_izdavanja,
            Publisher = izdavac,
            TypeOfPublisher = kategorija_izdavaca,
            EditionNumber = broj_izdanja,
            Description = opis,
            Language = jezik,
            Availability = dostupnost,
            UserId = id_korisnik,
            Genre = zanr
        };
    }
}