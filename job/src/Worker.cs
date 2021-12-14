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
                _logger.LogInformation($"{Environment.MachineName} Init Process Messages");
                await _processMessages.Process();
                _logger.LogInformation($"{Environment.MachineName} Final Process Messages");
                await Task.Delay(1000, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar");
            }
        }
    }
}
