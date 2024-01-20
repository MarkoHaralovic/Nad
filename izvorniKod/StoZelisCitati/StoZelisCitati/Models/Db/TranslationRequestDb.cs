namespace StoZelisCitati.Models.Db;

public record TranslationRequestDb(string naslov, string izdavac, int broj_zahtjeva)
{
    public TranslationRequest ToDomainObject() => new(naslov, izdavac, broj_zahtjeva);
}