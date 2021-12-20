using BusinessLogic.Interfaces;
using Data.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace BusinessLogic;
public class ProcessMessages : IProcessMessages
{
    private readonly ILogger<ProcessMessages> _logger;
    private readonly IAwsRepository _awsRepository;
    private readonly IPostgreSqlRepository _postgreSqlRepository;
    private readonly IAwsDynamoRepository<string> _awsDynamoRepository;
    public ProcessMessages(ILogger<ProcessMessages> logger,
        IAwsRepository awsRepository,
        IPostgreSqlRepository postgreSqlRepository,
        IAwsDynamoRepository<string> awsDynamoRepository)
    {
        _logger = logger;
        _awsRepository = awsRepository;
        _postgreSqlRepository = postgreSqlRepository;
        _awsDynamoRepository = awsDynamoRepository;
    }

    public async Task Process()
    {
        await Task.WhenAll(new[] { ProcessNormalMessages(), ProcessDlqMessages()});
    }

    private async Task ProcessNormalMessages()
    {
        int deletedMessages = 0;
        var messages = await _awsRepository.ReceiveMessagesAsync(false);
        if (!messages.Any())
        {
            _logger.LogInformation($"Receive empty messages");
            return;
        }
        
        _logger.LogInformation($"Receive {messages.Count} messages");

        try
        {
            var batchCreateMessages = messages.Select(message => (message.MessageId, message.Body));
            await _awsDynamoRepository.BatchSaveMessageAsync(batchCreateMessages);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Insert dynamo error");
        }
        
        foreach (var message in messages)
        {
            if (Guid.TryParse(message.Body, out var guid))
            {
                try
                {
                    await _postgreSqlRepository.InsertMessage(guid, Environment.MachineName);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Insert message error");
                    await _postgreSqlRepository.InsertErrorMessage(guid, ex.Message, Environment.MachineName);
                }
            }
            else
            {
                _logger.LogError($"Invalid message: {message}");
            }

            if (await _postgreSqlRepository.DeleteNormalMessage())
            {
                try
                {
                    await _awsDynamoRepository.DeleteMessageAsync(message.MessageId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Delete dynamo error");
                }
                
                await _awsRepository.DeleteMessageAsync(message.ReceiptHandle, false);
                deletedMessages++;
            }

        }
        _logger.LogInformation($"Receive {messages.Count} messages and delete {deletedMessages}");
    }

    private async Task ProcessDlqMessages()
    {
        int deletedMessages = 0;
        var messages = await _awsRepository.ReceiveMessagesAsync(true);
        if (!messages.Any())
        {
            _logger.LogInformation($"Receive empty messages from DLQ");
            return;
        }
        _logger.LogInformation($"Receive {messages.Count} messages from DLQ");
        foreach (var message in messages)
        {
            if (Guid.TryParse(message.Body, out var guid))
            {
                try
                {
                    await _postgreSqlRepository.InsertMessageDlq(guid, DateTime.Now.Minute, Environment.MachineName);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Insert DLQ message error");
                    await _postgreSqlRepository.InsertErrorMessage(guid, ex.Message, Environment.MachineName);
                }
            }
            else
            {
                _logger.LogError($"Invalid DLQ message {message}");
            }

            if (await _postgreSqlRepository.DeleteDlqMessage())
            {
                try
                {
                    await _awsDynamoRepository.DeleteMessageAsync(message.MessageId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Delete dynamo error on DLQ message");
                }
                await _awsRepository.DeleteMessageAsync(message.ReceiptHandle, true);
                deletedMessages++;
            }
        }
        _logger.LogInformation($"Receive {messages.Count} messages from DLQ and delete {deletedMessages}");
    }
}
