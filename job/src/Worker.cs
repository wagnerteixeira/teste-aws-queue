using Job.Configuration;
using Data.Repositories.Interfaces;
using CrossCutting.Models;

namespace Job;

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
        Console.WriteLine($"Arquivo /data/{Environment.MachineName}.txt");
        while (!stoppingToken.IsCancellationRequested)
        {
            var messages = await _awsRepository.ReceiveMessagesAsync();
            _totalMesssges+= messages.Count;
            _logger.LogInformation($"{Environment.MachineName} Receive total of {_totalMesssges} messages and not delete");
            foreach(var message in messages)
            {
                var model = new ModelFila(Environment.MachineName, new Guid(message.Body));
                Console.WriteLine(model.id);
                File.AppendAllText($"/data/{Environment.MachineName}.txt", $"{message.Body}" + Environment.NewLine);
            }
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
