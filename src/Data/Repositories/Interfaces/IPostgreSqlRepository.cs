namespace Data.Repositories.Interfaces;

public interface IPostgreSqlRepository
{
    Task<bool> InsertMessage(Guid guid, string machineName);
    Task<bool> InsertMessageDlq(Guid guid, int minute, string machineName);
    Task<bool> DeleteNormalMessage();
    Task<bool> DeleteDlqMessage();
    Task InsertErrorMessage(Guid guid, string errorMessage, string machineName);
}
