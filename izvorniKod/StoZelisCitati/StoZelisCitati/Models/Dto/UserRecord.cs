using System.ComponentModel.DataAnnotations;

namespace StoZelisCitati.Models.Dto;

public class UserRecord
{
    public int id_korisnik { get; set; }
    public string korisnicko_ime { get; set; }
    public string lozinka { get; set; }
    public string naziv_korisnika { get; set; }
    public string vrsta_korisnika { get; set; }
    [EmailAddress]
    public string email { get; set; }
    [Phone]
    public string broj_mobitela { get; set; }
    public string adresa { get; set; }
    public string grad { get; set; }
    public string drzava { get; set; }
    public bool odobren { get; set; }

    public User ToDomainObject()
    {
        return new User
        {
            Id = id_korisnik,
            Username = korisnicko_ime,
            Password = lozinka,
            DisplayName = naziv_korisnika,
            UserType = vrsta_korisnika switch
            {
                "antikvarijat" => UserType.Antiquarian,
                "preprodavač" => UserType.Middleman,
                "izdavač" => UserType.Publisher,
                "admin" => UserType.Admin,
                _ => throw new ArgumentOutOfRangeException(nameof(vrsta_korisnika))
            },
            Email = email,
            PhoneNumber = broj_mobitela,
            Address = adresa,
            City = grad,
            Country = drzava,
            Approved = odobren
        };
    }
}