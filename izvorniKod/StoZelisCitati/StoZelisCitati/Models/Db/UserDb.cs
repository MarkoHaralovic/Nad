using NpgsqlTypes;

namespace StoZelisCitati.Models.Db;

public record UserDb(
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
    public User ToDomainObject() =>
        new(id_korisnik,
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
            koordinate.X);
}