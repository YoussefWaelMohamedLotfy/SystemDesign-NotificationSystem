namespace Worker.SMSService;

public class SmsWorker : BackgroundService
{
    private readonly ILogger<SmsWorker> _logger;

    public SmsWorker(ILogger<SmsWorker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            await Task.Delay(1000, stoppingToken);
        }
    }
}
