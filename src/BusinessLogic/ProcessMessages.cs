﻿using BusinessLogic.Interfaces;
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
        var messages = await _awsRepository.ReceiveMessagesAsync(false);
        _logger.LogInformation($"{Environment.MachineName} Receive {messages.Count} messages");
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

            await _awsRepository.DeleteMessageAsync(message.ReceiptHandle, false);

        }
    }

    private async Task ProcessDlqMessages()
    {
        var messages = await _awsRepository.ReceiveMessagesAsync(true);
        _logger.LogInformation($"{Environment.MachineName} Receive {messages.Count} messages from DLQ");
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
            await _awsRepository.DeleteMessageAsync(message.ReceiptHandle, true);
        }
    }
}