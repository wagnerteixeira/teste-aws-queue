using System.Data;
using Dapper;
using Data.Repositories.Interfaces;

namespace Data.Repositories;
public class PostgreSqlRepository : IPostgreSqlRepository
{
    private readonly IDbConnection _connection;
    public PostgreSqlRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<bool> InsertMessage(Guid guid)
    {
        var sqlStatement = "INSERT INTO public.message (id) VALUES(@Id);";
        return (await _connection.ExecuteAsync(sqlStatement, new { Id = guid })) > 0;
    }

    public async Task<bool> InsertMessageDlq(Guid guid, int minute)
    {
        var sqlStatement = "INSERT INTO public.message_dlq (id, minute) VALUES(@Id, @Minute);";
        return (await _connection.ExecuteAsync(sqlStatement, new { Id = guid, Minute = minute })) > 0;
    }
}
