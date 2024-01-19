using Dapper;
using Npgsql;
using StoZelisCitati.Helpers;
using StoZelisCitati.Models;
using StoZelisCitati.Models.Db;

namespace StoZelisCitati.Misc;

public class NpgsqlRepository
{
    private readonly NpgsqlConnection npgsqlConnection;
    public NpgsqlRepository(NpgsqlConnection npgsqlConnection)
    {
        this.npgsqlConnection = npgsqlConnection;
    }

    public async Task AddTranslationRequest(int userId, int bookId)
    {
        string query = """
                       insert into zahtjevi_za_prijevodom
                       values (@bookId, @userId, 1)
                       on conflict (id_knjiga, id_korisnik)
                       do update set broj_zahtjeva = zahtjevi_za_prijevodom.broj_zahtjeva + 1
                       """;

        await npgsqlConnection.ExecuteAsync(query, new {bookId, userId});
    }
    
    public async Task<Offer?> AddOffer(AddOfferRequest addOfferRequest)
    {
        string query = """
                       insert into ponuda
                       values
                       (
                        default,
                        @price,
                        @state,
                        @count,
                        @titleId
                       ) returning *
                       """;

        return (await npgsqlConnection.QuerySingleOrDefaultAsync<OfferDb>(query, addOfferRequest))
            ?.ToDomainObject();
    }
    
    public async Task AddBookCover(byte[] cover, string imageType, int titleId)
    {
        string query = """
                       insert into korice
                       values
                       (
                        default,
                        @cover,
                        @titleId,
                        @imageType
                       )
                       """;
        
        await npgsqlConnection.ExecuteAsync(query, new {cover, imageType, titleId});
    }
    
    /// <returns>The id of the inserted title.</returns>
    public async Task<int> AddBook(AddBookRequest addBookRequest, int userId)
    {
        string query = """
                       insert into knjiga
                       values
                       (
                        default,
                        @author,
                        @year,
                        @publisher,
                        @typeOfPublisher,
                        @genre,
                        @isbn,
                        @edition,
                        @description,
                        @language,
                        @availability,
                        @userId,
                        @title
                       ) returning id_knjiga
                       """;
        
        return await npgsqlConnection.QuerySingleAsync<int>(query,
            new DynamicParameters().Append(addBookRequest, new{userId}));
    }
    
    public async Task AddUser(RegisterRequest registerRequest, double latitude, double longitude)
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
                        false,
                        point(@longitude, @latitude)
                       )
                       """;

        await npgsqlConnection.ExecuteAsync(query,
            new DynamicParameters().Append(registerRequest, new {latitude, longitude}));
    }

    public async Task<IEnumerable<TranslationRequest>> GetTranslationRequests(int userId)
    {
        string query = """
                       select naslov, izdavac, broj_zahtjeva
                       from zahtjevi_za_prijevodom join knjiga on zahtjevi_za_prijevodom.id_knjiga = knjiga.id_knjiga
                       where zahtjevi_za_prijevodom.id_korisnik = @userId
                       """;

        return (await npgsqlConnection.QueryAsync<TranslationRequestDb>(query, new {userId}))
            .Select(x => x.ToDomainObject());
    }

    public async Task<IEnumerable<Offer>> GetOffers(int bookId)
    {
        string query = "select * from ponuda where id_knjiga = @bookId";

        return (await npgsqlConnection.QueryAsync<OfferDb>(query, new {bookId}))
            .Select(x => x.ToDomainObject());
    }

    public async Task<Offer?> GetOffer(int offerId)
    {
        string query = "select * from ponuda where id_ponuda = @offerId";

        return (await npgsqlConnection.QuerySingleOrDefaultAsync<OfferDb>(query, new {offerId}))
            ?.ToDomainObject();
    }

    public async Task UpdateOffer(int offerId, double price, string state, int count)
    {
        string query = """
                       update ponuda set cijena = @price, stanje = @state, broj_primjeraka = @count
                       where id_ponuda = @offerId
                       """;

        await npgsqlConnection.ExecuteAsync(query, new {offerId, price, state, count});
    }

    public async Task DeleteOffer(int offerId)
    {
        string query = """
                       delete from ponuda where id_ponuda = @offerId
                       """;

        await npgsqlConnection.ExecuteAsync(query, new {offerId});
    }

    /// <returns>the id of the user that posted the offer</returns>
    public async Task<int?> GetOwnerOfOffer(int offerId)
    {
        string query = """
                       select id_korisnik
                       from ponuda natural join knjiga natural join korisnik
                       where id_ponuda = @offerId
                       """;

        return await npgsqlConnection.QuerySingleOrDefaultAsync<int>(query, new {offerId});
    }
    
    public async Task<Book?> GetBookWithId(int bookId)
    {
        string query = "select * from knjiga where id_knjiga = @bookId";

        return (await npgsqlConnection.QuerySingleOrDefaultAsync<BookDb>(query, new {bookId}))
            ?.ToDomainObject();
    }
    
    public async Task<BookCover?> GetBookCoverForBook(int bookId)
    {
        string query = "select * from korice where id_knjiga = @bookId";
        
        return (await npgsqlConnection.QuerySingleOrDefaultAsync<BookCoverDb>(query, new {bookId}))
            ?.ToDomainObject();
    }

    public async Task<int?> GetOwnerOfBook(int bookId)
    {
        string query = "select id_korisnik from knjiga where id_knjiga = @bookId";
        return await npgsqlConnection.QuerySingleOrDefaultAsync<int>(query, new {bookId});
    }

    public async Task<int> GetNumberOfBooksBelongingToUser(int userId)
    {
        string query = "select count(*) from knjiga where id_korisnik = @userId";
        return await npgsqlConnection.QuerySingleAsync<int>(query, new {userId});
    }
    public async Task<IEnumerable<(Book, Offer)>> GetBooksBelongingToUser(int userId)
    {
        string query = """
                       select knjiga.*, ponuda.*
                       from knjiga natural join ponuda
                       where id_korisnik = @userId
                       """;

        return await npgsqlConnection.QueryAsync<BookDb, OfferDb, (Book, Offer)>(
            query,
            (b, o) => (b.ToDomainObject(), o.ToDomainObject()),
            new {userId},
            splitOn: "id_ponuda");
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

        return (await npgsqlConnection.QuerySingleOrDefaultAsync<UserDb>(query, new {username}))
            ?.ToDomainObject();
    }
    
    public async Task<User?> GetUser(int userId)
    {
        string query = """
                       select *
                       from korisnik
                       where id_korisnik = @userId
                       """;

        return (await npgsqlConnection.QuerySingleOrDefaultAsync<UserDb>(query, new {userId}))
            ?.ToDomainObject();
    }
    
    public async Task<IEnumerable<User>> GetUsers(bool approved)
    {
        string query = """
                       select *
                       from korisnik
                       where odobren = @approved
                       """;

        return (await npgsqlConnection.QueryAsync<UserDb>(query, new {approved}))
            .Select(x => x.ToDomainObject());
    }
    
    public async Task<IEnumerable<User>> GetUsers(string userType)
    {
        string query = """
                       select *
                       from korisnik
                       where odobren = true and vrsta_korisnika = @userType
                       """;

        return (await npgsqlConnection.QueryAsync<UserDb>(query, new {userType}))
            .Select(x => x.ToDomainObject());
    }
    
    public async Task<(IEnumerable<(Book, Offer)> books, int pageCount)> FilterBooks(BookQuery bookQuery)
    {
        int booksPerPage = 5;
        
        var builder = new SqlBuilder();

        string query = """
                       select knjiga.*, ponuda.*
                       from korisnik natural join knjiga natural join ponuda
                       /**where**/
                       /**orderby**/
                       limit @booksPerPage
                       offset @offset
                       """;
        
        var selectTemplate = builder.AddTemplate(query,
            new { offset = (bookQuery.Page - 1) * booksPerPage , booksPerPage });
        
        var countTemplate = builder
            .AddTemplate("select count(*) from korisnik natural join knjiga natural join ponuda /**where**/");

        int LevenshteinBound(int searchLength) => Math.Max(searchLength / 2, 3);

        List<string> levenSum = new();
        if (bookQuery.Title != null)
        {
            int titleLen = bookQuery.Title.Length;
            builder.Where("levenshtein(left(naslov, @titleLen), @title) <= @titleBound",
                new {title = bookQuery.Title, titleLen, titleBound = LevenshteinBound(titleLen)});
            levenSum.Add("levenshtein(left(naslov, @titleLen), @title)");
        }
        if (bookQuery.Author != null)
        {
            int authorLen = bookQuery.Author.Length;
            builder.Where("levenshtein(left(autor, @authorLen), @author) <= @authorBound",
                new {author = bookQuery.Author, authorLen, authorBound = LevenshteinBound(authorLen)});
            levenSum.Add("levenshtein(left(autor, @authorLen), @author)");
        }
        if (bookQuery.Publisher != null)
        {
            int publisherLen = bookQuery.Publisher.Length;
            builder.Where("levenshtein(left(izdavac, @publisherLen), @publisher) <= @publisherBound",
                new {publisher = bookQuery.Publisher, publisherLen, publisherBound = LevenshteinBound(publisherLen)});
            levenSum.Add("levenshtein(left(izdavac, @publisherLen), @publisher)");
        }
        if (bookQuery.Genre != null)
        {
            int genreLen = bookQuery.Genre.Length;
            builder.Where("levenshtein(left(zanr, @genreLen), @genre) <= @genreBound",
                new {genre = bookQuery.Genre, genreLen, genreBound = LevenshteinBound(genreLen)});
            levenSum.Add("levenshtein(left(zanr, @genreLen), @genre)");
        }
        if (bookQuery.Isbn != null)
        {
            int isbnLen = bookQuery.Isbn.Length;
            builder.Where("levenshtein(left(isbn, @isbnLen), @isbn) <= @isbnBound",
                new {isbn = bookQuery.Isbn, isbnLen, isbnBound = LevenshteinBound(isbnLen)});
            levenSum.Add("levenshtein(left(isbn, @isbnLen), @isbn)");
        }
        if (bookQuery.Language != null)
        {
            int languageLen = bookQuery.Language.Length;
            builder.Where("levenshtein(left(jezik, @languageLen), @language) <= @languageBound",
                new {language = bookQuery.Language, languageLen, languageBound = LevenshteinBound(languageLen)});
            levenSum.Add("levenshtein(left(jezik, @languageLen), @language)");
        }
        if (bookQuery.Seller != null)
        {
            int sellerLen = bookQuery.Seller.Length;
            builder.Where("levenshtein(left(naziv_korisnika, @sellerLen), @seller) <= @sellerBound",
                new {seller = bookQuery.Seller, sellerLen, sellerBound = LevenshteinBound(sellerLen)});
            levenSum.Add("levenshtein(left(naziv_korisnika, @sellerLen), @seller)");
        }
        if (levenSum.Count != 0)
            builder.OrderBy(string.Join(" + ", levenSum));
        
        
        if (bookQuery.YearFrom != null)
            builder.Where("godina_izdavanja > @yearFrom", new {yearFrom = bookQuery.YearFrom});
        if (bookQuery.YearTo != null)
            builder.Where("godina_izdavanja < @yearTo", new {yearTo = bookQuery.YearTo});
        if (bookQuery.Edition != null)
            builder.Where("broj_izdanja = @edition", new {edition = bookQuery.Edition});
        if (bookQuery.TypeOfPublisher != null)
            builder.Where("kategorija_izdavaca = @pType", new {pType = bookQuery.TypeOfPublisher});
        if (bookQuery.Availability != null)
            builder.Where("dostupnost = @availability", new {availability = bookQuery.Availability});
        if (bookQuery.State != null)
            builder.Where("stanje = @state", new {state = bookQuery.State});
        if (bookQuery.PriceFrom != null)
            builder.Where("cijena > @priceFrom", new {priceFrom = bookQuery.PriceFrom});
        if (bookQuery.PriceTo != null)
            builder.Where("cijena < @priceTo", new {priceTo = bookQuery.PriceTo});
        
        
        IEnumerable<(Book, Offer)> books = await npgsqlConnection.QueryAsync<BookDb, OfferDb, (Book, Offer)>(
                selectTemplate.RawSql,
                (b, o) => (b.ToDomainObject(), o.ToDomainObject()),
                selectTemplate.Parameters,
                splitOn:"id_ponuda");

        int count = await npgsqlConnection.QuerySingleAsync<int>(countTemplate.RawSql, countTemplate.Parameters);
        int pageCount = (int) Math.Ceiling(count / (decimal) booksPerPage);
        
        return (books, pageCount);
    }
}