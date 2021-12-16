using BusinessLogic.Interfaces;
using Data.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace BusinessLogic;
public class ProcessMessages : IProcessMessages
{
    private readonly ILogger<ProcessMessages> _logger;
    private readonly IAwsRepository _awsRepository;
    private readonly IPostgreSqlRepository _postgreSqlRepository;
    public ProcessMessages(ILogger<ProcessMessages> logger,
        IAwsRepository awsRepository,
        IPostgreSqlRepository postgreSqlRepository)
    {
        _logger = logger;
        _awsRepository = awsRepository;
        _postgreSqlRepository = postgreSqlRepository;
    }

    public async Task Process()
    {
        await Task.WhenAll(new[] { ProcessNormalMessages(), ProcessDlqMessages() });
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
        foreach (var message in messages)
        {
            if (Guid.TryParse(message.Body, out var guid))
            {
                await _postgreSqlRepository.InsertMessage(guid);
            }
            else
            {
                _logger.LogError($"Mensagem {message} inválida");
            }

            if (await _postgreSqlRepository.DeleteNormalMessage())
            {
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
                await _postgreSqlRepository.InsertMessageDlq(guid, DateTime.Now.Minute);
            }
            else
            {
                _logger.LogError($"Mensagem {message} inválida");
            }

            if (await _postgreSqlRepository.DeleteDlqMessage())
            {
                await _awsRepository.DeleteMessageAsync(message.ReceiptHandle, true);
                deletedMessages++;
            }
        }
        _logger.LogInformation($"Receive {messages.Count} messages from DLQ and delete {deletedMessages}");
    }
}
