using Microsoft.Extensions.DependencyInjection;
using LoggerApi.Services;
using LoggerApi.Models;

public class DataTransferService : BackgroundService
{
    private readonly ILogger<DataTransferService> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public DataTransferService(ILogger<DataTransferService> logger, IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Datenübertragung gestartet: {time}", DateTimeOffset.Now);
            await TransferDataAsync();
            _logger.LogInformation("Datenübertragung abgeschlossen: {time}", DateTimeOffset.Now);

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }

    private async Task TransferDataAsync()
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var logEntryService = scope.ServiceProvider.GetRequiredService<LogEntriesService>();

            var sourceEntries = await logEntryService.GetSourceLogEntriesAsync();

            foreach (var entry in sourceEntries)
            {
                if (!await logEntryService.EntryExistsInDestinationAsync(entry.Id))
                {
                    await logEntryService.CopyLogEntryToDestinationAsync(entry.Id);
                }
            }
        }
    }
}