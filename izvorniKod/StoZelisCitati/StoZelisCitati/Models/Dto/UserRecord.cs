using System.ComponentModel.DataAnnotations;
using NpgsqlTypes;
using StoZelisCitati.Helpers;

namespace StoZelisCitati.Models.Dto;

public record UserRecord(
    int id_korisnik,
    string korisnicko_ime,
    string lozinka,
    string naziv_korisnika,
    string vrsta_korisnika,
    string email,
    string broj_mobitela,
    string adresa,
    string grad,
    string drzava,
    bool odobren,
    NpgsqlPoint koordinate)
{
    public User ToDomainObject()
    {
        return new User(
            id_korisnik,
            korisnicko_ime,
            lozinka,
            naziv_korisnika,
            vrsta_korisnika,
            email,
            broj_mobitela,
            adresa,
            grad,
            drzava,
            odobren,
            koordinate.Y,
            koordinate.X
        );
    }
}