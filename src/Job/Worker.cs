using BusinessLogic.Interfaces;

namespace Job;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IProcessMessages _processMessages;

    public Worker(ILogger<Worker> logger,
        IProcessMessages processMessages)
    {
        _logger = logger;
        _processMessages = processMessages;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogInformation($"Init Process Messages");
                await Task.Delay(Random.Shared.Next(1000, 2000), stoppingToken);
                await _processMessages.Process();
                _logger.LogInformation($"Final Process Messages");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Process messages error");
            }
        }
    }
}
