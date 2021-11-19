using Amazon.SQS.Model;
using api.Repositories.Models;

namespace api.Repositories.Interfaces
{
    public interface IAwsRepository
    {
        Task<bool> SendMessageAsync(string message);
        Task<List<Message>> ReceiveMessagesAsync();
        Task<bool> DeleteMessageAsync(string messageReceiptHandle);
        Task<BatchMessageResults> DeleteMessagesAsync(IEnumerable<Message> messages);
        Task<BatchMessageResults> SendMessagesAsync(IEnumerable<string> messages);
    }
}