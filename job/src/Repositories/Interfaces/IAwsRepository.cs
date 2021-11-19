using Amazon.SQS.Model;
using job.Repositories.Models;

namespace job.Repositories.Interfaces
{
    public interface IAwsRepository
    {
        Task<bool> SendMessageAsync(string message);
        Task<List<Message>> ReceiveMessagesAsync();
        Task<bool> DeleteMessageAsync(string messageReceiptHandle);
        Task<DeleteMessageResults> DeleteMessagesAsync(IEnumerable<Message> messages);
    }
}