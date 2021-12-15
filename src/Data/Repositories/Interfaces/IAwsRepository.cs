using Amazon.SQS.Model;
using Data.Repositories.Models;

namespace Data.Repositories.Interfaces
{
    public interface IAwsRepository
    {
        Task<bool> SendMessageAsync(string message);
        Task<List<Message>> ReceiveMessagesAsync(bool dlq);
        Task<bool> DeleteMessageAsync(string messageReceiptHandle, bool dlq);
        Task<BatchMessageResults> DeleteMessagesAsync(IEnumerable<Message> messages, bool dlq);
        Task<BatchMessageResults> SendMessagesAsync(IEnumerable<string> messages);
    }
}