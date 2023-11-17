namespace StoZelisCitati.Models;

public class Book
{
    public string title;
    public string author;
    public string isbn;
    public string publisher;

    public Book(string title, string author, string isbn, string publisher)
    {
        this.title = title;
        this.author = author;
        this.isbn = isbn;
        this.publisher = publisher;
    }
}