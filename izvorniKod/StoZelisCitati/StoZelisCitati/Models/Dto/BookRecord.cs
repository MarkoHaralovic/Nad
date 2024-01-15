namespace StoZelisCitati.Models.Dto;

public record BookRecord(
    int id_knjiga,
    string naslov,
    string autor,
    string isbn,
    int godina_izdavanja,
    string izdavac,
    string kategorija_izdavaca,
    int broj_izdanja,
    string opis,
    string jezik,
    string dostupnost,
    int id_korisnik,
    string zanr)
{
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
            Edition = broj_izdanja,
            Description = opis,
            Language = jezik,
            Availability = dostupnost,
            UserId = id_korisnik,
            Genre = zanr
        };
    }
}