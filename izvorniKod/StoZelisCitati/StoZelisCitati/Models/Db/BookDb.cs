namespace StoZelisCitati.Models.Db;

public record BookDb(
    int id_knjiga,
    string autor,
    int godina_izdavanja,
    string izdavac,
    string kategorija_izdavaca,
    string zanr,
    string isbn,
    int broj_izdanja,
    string opis,
    string jezik,
    string dostupnost,
    int id_korisnik,
    string naslov)
{
    public Book ToDomainObject() =>
        new(id_knjiga,
            naslov,
            autor,
            isbn,
            godina_izdavanja,
            izdavac,
            kategorija_izdavaca,
            broj_izdanja,
            opis,
            jezik,
            dostupnost,
            id_korisnik,
            zanr);
}