using EventBus.RabbitMQ.Events;
using MassTransit;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Worker.EmailService.Consumers;

internal class SendNotificationConsumer : IConsumer<SendNotificationEvent>
{
    private readonly ISendGridClient _emailClient;
    private readonly ILogger<SendNotificationConsumer> _logger;

    public SendNotificationConsumer(ISendGridClient emailClient, ILogger<SendNotificationConsumer> logger)
    {
        _emailClient = emailClient ?? throw new ArgumentNullException(nameof(emailClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Consume(ConsumeContext<SendNotificationEvent> context)
    {
        SendGridMessage emailMessage = new()
        {
            Subject = "Notification Received via SendGrid",
            From = new() { Email = "youssefwael8@gmail.com", Name = "Joe Developer" },
        };

        emailMessage.AddTo(new EmailAddress(context.Message.EmailAddress));
        emailMessage.AddContent(MimeType.Text, "and easy to do anywhere, even with C#");
        var response = await _emailClient.SendEmailAsync(emailMessage).ConfigureAwait(false);

        if (response.IsSuccessStatusCode)
            _logger.LogInformation("SendGrid Response: {response}", response.StatusCode);
        else
            _logger.LogError("SendGrid Response: {response}", response.StatusCode);

        _logger.LogInformation("Consumed Event: {event}", context.Message);
    }
}
