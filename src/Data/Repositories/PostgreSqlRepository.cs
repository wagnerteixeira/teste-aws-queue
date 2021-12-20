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

    public async Task<bool> InsertMessage(Guid guid, string machineName, DateTime processTime)
    {
        var sqlStatement = "INSERT INTO public.message (id, machine_name, process_time) VALUES(@Id, @MachineName, @ProcessTime);";
        return (await _connection.ExecuteAsync(sqlStatement, new { Id = guid, MachineName = machineName, ProcessTime = processTime })) > 0;
    }

    public async Task<bool> InsertMessageDlq(Guid guid, int minute, string machineName, DateTime processTime)
    {
        var sqlStatement = "INSERT INTO public.message_dlq (id, minute, machine_name, process_time) VALUES(@Id, @Minute, @MachineName, @ProcessTime);";
        return (await _connection.ExecuteAsync(sqlStatement, new { Id = guid, Minute = minute, MachineName = machineName, ProcessTime = processTime })) > 0;
    }

    public async Task<bool> DeleteNormalMessage()
    {
        var sqlStatement = "select delete_normal_message from \"control\" limit 1";
        return await _connection.QueryFirstAsync<bool>(sqlStatement);
    }

    public async Task<bool> DeleteDlqMessage()
    {
        var sqlStatement = "select delete_dlq_message from \"control\" limit 1";
        return await _connection.QueryFirstAsync<bool>(sqlStatement);
    }

    public async Task InsertErrorMessage(Guid guid, string errorMessage, string machineName, DateTime processTime)
    {
        var sqlStatement = "INSERT INTO public.errors (id, message, machine_name, process_time) VALUES(@Id, @Message, @MachineName, @ProcessTime);";
        await _connection.ExecuteAsync(sqlStatement, new { Id = guid, Message = errorMessage, MachineName = machineName, ProcessTime = processTime });
    }
}
