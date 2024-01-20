namespace StoZelisCitati.Models;

public record BookCover(int Id, byte[] Image, string ImageType, int BookId);