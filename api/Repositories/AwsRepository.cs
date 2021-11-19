
using Amazon.SQS;
using Amazon.SQS.Model;
using api.Configuration;
using api.Repositories.Interfaces;
using api.Repositories.Models;

namespace api.Repositories
{
    public class AwsRepository : IAwsRepository
    {
        private readonly IAmazonSQS _sqs;
        private readonly AppSettings _appSettings;
        private readonly ILogger<AwsRepository> _logger;
        public AwsRepository(
           IAmazonSQS sqs,
           AppSettings appSettings,
           ILogger<AwsRepository> logger)
        {
            _sqs = sqs;
            _appSettings = appSettings;
            _logger = logger;
        }
        public async Task<bool> SendMessageAsync(string message)
        {
            try
            {
                var sendRequest = new SendMessageRequest(_appSettings.QueueUrl, message);
                var sendResult = await _sqs.SendMessageAsync(sendRequest);

                return sendResult.HttpStatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error with send aws message", message);
                throw;
            }
        }

        public async Task<BatchMessageResults> SendMessagesAsync(IEnumerable<string> messages)
        {
            try
            {
                var sendMessagesBatchRequest = messages.Select(m => new SendMessageBatchRequestEntry(Guid.NewGuid().ToString(), m)).ToList();
                var result = await _sqs.SendMessageBatchAsync(_appSettings.QueueUrl, sendMessagesBatchRequest);

                var success = result.Successful.Select(s => s.Id).ToArray();
                var fails = result.Failed.Select(s => s.Id).ToArray();
                BatchMessageResults results = new(success, fails);
                return results;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error with send aws messages", messages);
                throw;
            }
        }
        public async Task<List<Message>> ReceiveMessagesAsync()
        {
            try
            {
                var request = new ReceiveMessageRequest
                {
                    QueueUrl = _appSettings.QueueUrl,
                    MaxNumberOfMessages = 10,
                    WaitTimeSeconds = 5
                };
                var result = await _sqs.ReceiveMessageAsync(request);

                return result.Messages.Any() ? result.Messages : new List<Message>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error with receive messages");
                throw;
            }
        }
        public async Task<bool> DeleteMessageAsync(string messageReceiptHandle)
        {
            try
            {
                var deleteResult = await _sqs.DeleteMessageAsync(_appSettings.QueueUrl, messageReceiptHandle);
                return deleteResult.HttpStatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error with delete message", messageReceiptHandle);
                throw;
            }
        }

        public async Task<BatchMessageResults> DeleteMessagesAsync(IEnumerable<Message> messages)
        {
            try
            {
                var deleteMessagesBatchRequest = messages.Select(message => new DeleteMessageBatchRequestEntry(message.MessageId, message.ReceiptHandle)).ToList();
                var result = await _sqs.DeleteMessageBatchAsync(_appSettings.QueueUrl, deleteMessagesBatchRequest);
                var success = result.Successful.Select(s => s.Id).ToArray();
                var fails = result.Failed.Select(s => s.Id).ToArray();
                BatchMessageResults results = new(success, fails);
                return results;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error with delete messages", messages);
                throw;
            }
        }
    }
}