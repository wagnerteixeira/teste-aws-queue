using job.Configuration;
using job.Repositories.Interfaces;

namespace job;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IAwsRepository _awsRepository;

    private int _totalMesssges = 0;

    public Worker(ILogger<Worker> logger, AppSettings appSettings, IAwsRepository awsRepository)
    {
        _logger = logger;
       _awsRepository = awsRepository;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var messages = await _awsRepository.ReceiveMessagesAsync();
            _totalMesssges+= messages.Count;
            _logger.LogInformation($"Receive total of {_totalMesssges} messages and not delete");
            // foreach(var message in messages)
            // {
            //     _logger.LogInformation("Message receive", message.Body);
            //     _logger.LogInformation("Message attributes", message.MessageAttributes);
            //    // await _awsRepository.DeleteMessageAsync(message.ReceiptHandle);
            //    //_logger.LogInformation("Message deleted", message.ReceiptHandle);
            // }
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            await Task.Delay(1000, stoppingToken);
        }
    }
}
