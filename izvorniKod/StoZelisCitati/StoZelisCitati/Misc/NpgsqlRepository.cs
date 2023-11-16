using Dapper;
using MvcTest.Models;
using MvcTest.Models.Dto;
using Npgsql;

namespace MvcTest.Misc;

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

        return (await npgsqlConnection.QueryAsync<Korisnik>(query)).Select(x => x.ToDomainObject());
    }

    public async Task AddUser(string userName, string password, string displayName, string userType,
        string email, string phoneNumber, string address, bool approved)
    {
        string query = """
                       insert into korisnik
                       values 
                       (
                               default,
                               @userName,
                               @password,
                               @displayName,
                               @userType,
                               @email,
                               @phoneNumber,
                               @address,
                               @approved
                       )
                       """;

        await npgsqlConnection.ExecuteAsync(query, 
            new {userName, password, displayName, userType, email, phoneNumber, address, approved});
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
}