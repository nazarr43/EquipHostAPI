using EquipHostAPI.Application.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EquipHostAPI.Application;
public class ContractBackgroundProcessor : BackgroundService, IContractBackgroundProcessor
{
    private readonly ILogger<ContractBackgroundProcessor> _logger;

    public ContractBackgroundProcessor(ILogger<ContractBackgroundProcessor> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Background task is running...");
            await Task.Delay(5000, stoppingToken);
        }
    }
    public void ProcessContractCreated(int contractId)
    {
        _logger.LogInformation($"Contract {contractId} was created, processing it in the background...");
    }
}

