namespace Data.Repositories.Interfaces;

public interface IPostgreSqlRepository
{
    Task<bool> InsertMessage(Guid guid);
    Task<bool> InsertMessageDlq(Guid guid, int minute);
    Task<bool> DeleteNormalMessage();
    Task<bool> DeleteDlqMessage();
}
