using Dapper;
using Npgsql;
using StoZelisCitati.Models;
using StoZelisCitati.Models.Dto;

namespace StoZelisCitati.Misc;

public class NpgsqlRepository
{
    private readonly NpgsqlConnection npgsqlConnection;
    public NpgsqlRepository(NpgsqlConnection npgsqlConnection)
    {
        this.npgsqlConnection = npgsqlConnection;
    }

    public async Task<IEnumerable<User>> GetUnapprovedUsers()
    {
        string query = """
                       select * from korisnik
                       where odobren = false
                       """;

        return (await npgsqlConnection.QueryAsync<UserRecord>(query)).Select(x => x.ToDomainObject());
    }

    public async Task AddUser(string username, string password, string displayName, string userType,
        string email, string phoneNumber, string address, string city, string country, bool approved)
    {
        string query = """
                       insert into korisnik
                       values
                       (
                               default,
                               @username,
                               @password,
                               @displayName,
                               @userType,
                               @email,
                               @phoneNumber,
                               @address,
                               @city,
                               @country,
                               @approved
                       )
                       """;

        await npgsqlConnection.ExecuteAsync(query,
            new {username, password, displayName, userType, email, phoneNumber, address, city, country, approved});
    }
    
    public async Task ApproveUser(int userId)
    {
        string query = """
                       update korisnik
                       set odobren = true
                       where id_korisnik = @userId
                       """;

        await npgsqlConnection.ExecuteAsync(query, new {userId});
    }
    
    public async Task DeleteUser(int userId)
    {
        string query = """
                       delete from korisnik
                       where id_korisnik = @userId
                       """;

        await npgsqlConnection.ExecuteAsync(query, new {userId});
    }
    
    public async Task<User?> GetUser(string username)
    {
        string query = """
                       select *
                       from korisnik
                       where korisnicko_ime = @username
                       """;

        UserRecord? user = await npgsqlConnection.QuerySingleOrDefaultAsync<UserRecord>(query, new {username});
        return user?.ToDomainObject();
    }

    public async Task<IEnumerable<Book>> GetBooksWithGenre(string genre)
    {
        string query = """
                       select *
                       from knjiga
                       where zanr = @genre
                       """;
        return (await npgsqlConnection.QueryAsync<BookRecord>(query, new {genre}))
            .Select(x => x.ToDomainObject());
    }

    //this should also return the page count
    public async Task<(IEnumerable<BookListElementRecord> books, int pageCount)> FilterBooks(BookQuery bookQuery)
    {
        int booksPerPage = 5;
        
        var builder = new SqlBuilder();

        string query = """
                       select id_knjiga, naslov, autor, zanr, izdavac
                       from knjiga natural join ponuda
                       /**where**/
                       order by naslov, godina_izdavanja desc
                       limit @booksPerPage
                       offset @offset
                       """;
        
        var selectTemplate = builder.AddTemplate(query,
            new { offset = (bookQuery.Page - 1) * booksPerPage , booksPerPage });

        var countTemplate = builder.AddTemplate("select count(*) from knjiga natural join ponuda /**where**/");
        
        if (bookQuery.Title != null)
            builder.Where("naslov = @title", new {title = bookQuery.Title});
        if (bookQuery.YearFrom != null)
            builder.Where("godina_izdavanja > @yearFrom", new {yearFrom = bookQuery.YearFrom});
        if (bookQuery.YearTo != null)
            builder.Where("godina_izdavanja < @yearTo", new {yearTo = bookQuery.YearTo});
        if (bookQuery.Author != null)
            builder.Where("autor = @author", new {author = bookQuery.Author});
        if (bookQuery.Publisher != null)
            builder.Where("izdavac = @publisher", new {publisher = bookQuery.Publisher});
        if (bookQuery.Edition != null)
            builder.Where("broj_izdanja = @edition", new {edition = bookQuery.Edition});
        if (bookQuery.TypeOfPublisher != "")
            builder.Where("kategorija_izdavaca = @pType", new {pType = bookQuery.TypeOfPublisher});
        if (bookQuery.Genre != null)
            builder.Where("zanr = @genre", new {genre = bookQuery.Genre});
        if (bookQuery.Isbn != null)
            builder.Where("isbn = @isbn", new {isbn = bookQuery.Isbn});
        if (bookQuery.Language != null)
            builder.Where("jezik = @language", new {language = bookQuery.Language});
        if (bookQuery.Availability != "")
            builder.Where("dostupnost = @availability", new {availability = bookQuery.Availability});
        if (bookQuery.State != "")
            builder.Where("stanje = @state", new {state = bookQuery.State});

        IEnumerable<BookListElementRecord> books =
            await npgsqlConnection.QueryAsync<BookListElementRecord>(selectTemplate.RawSql, selectTemplate.Parameters);

        int count = await npgsqlConnection.QuerySingleAsync<int>(countTemplate.RawSql, countTemplate.Parameters);
        int pageCount = (int) Math.Ceiling(count / (decimal) booksPerPage);
        return (books, pageCount);
    }
}