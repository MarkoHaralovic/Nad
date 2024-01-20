namespace StoZelisCitati.Models.Db;

public record BookCoverDb(int id_korica, byte[] slika, int id_knjiga, string tip_slike)
{
    public BookCover ToDomainObject() => new(id_korica, slika, tip_slike, id_knjiga);
}