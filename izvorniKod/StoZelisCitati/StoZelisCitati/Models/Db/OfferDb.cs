namespace StoZelisCitati.Models.Db;

public record OfferDb(int id_ponuda, double cijena, string stanje, int broj_primjeraka, int id_knjiga)
{
    public Offer ToDomainObject() => new(id_ponuda, cijena, stanje, broj_primjeraka, id_knjiga);
}