using System.ComponentModel.DataAnnotations;
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
    bool odobren)
{
    public User ToDomainObject()
    {
        return new User
        {
            Id = id_korisnik,
            Username = korisnicko_ime,
            Password = lozinka,
            DisplayName = naziv_korisnika,
            UserType = vrsta_korisnika,
            Email = email,
            PhoneNumber = broj_mobitela,
            Address = adresa,
            City = grad,
            Country = drzava,
            Approved = odobren
        };
    }
}