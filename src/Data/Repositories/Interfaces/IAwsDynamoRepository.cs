using Amazon.SQS.Model;
using Data.Repositories.Models;

namespace Data.Repositories.Interfaces
{
    public interface IAwsDynamoRepository<T>
    {
        Task SaveMessageAsync(string id, T message);
        Task DeleteMessageAsync(string id);
        Task BatchSaveMessageAsync(IEnumerable<(string id, T message)> messages);
        Task BatchDeleteMessageAsync(List<string> ids);
    }
}