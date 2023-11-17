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
}