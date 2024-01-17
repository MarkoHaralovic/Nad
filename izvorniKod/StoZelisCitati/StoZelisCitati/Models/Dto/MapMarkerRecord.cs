using NpgsqlTypes;

namespace StoZelisCitati.Models.Dto;

public record MapMarkerRecord(string naziv_korisnika, NpgsqlPoint koordinate)
{
    public MapMarker ToDomainObject() => new(naziv_korisnika, koordinate.Y, koordinate.X);
}