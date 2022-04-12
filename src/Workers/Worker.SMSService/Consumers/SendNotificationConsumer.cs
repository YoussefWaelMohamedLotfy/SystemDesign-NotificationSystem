using EventBus.RabbitMQ.Events;
using MassTransit;
using Microsoft.Extensions.Options;
using Twilio.Rest.Api.V2010.Account;
using Worker.SMSService.Models;

namespace Worker.SMSService.Consumers;

internal class SendNotificationConsumer : IConsumer<SendNotificationEvent>
{
    private readonly TwilioOptions _twilioOptions;
    private readonly ILogger<SendNotificationConsumer> _logger;

    public SendNotificationConsumer(IOptions<TwilioOptions> twilioOptions, ILogger<SendNotificationConsumer> logger)
    {
        _twilioOptions = twilioOptions.Value;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Consume(ConsumeContext<SendNotificationEvent> context)
    {
        if (context.Message.Settings.Channel != "SMS" ||
            context.Message.Settings.IsOptIn == false)
        {
            _logger.LogInformation("Event Message Ignored...");
            return;
        }

        var messageDetails = await MessageResource.CreateAsync(
                from: _twilioOptions.FromPhoneNumber,
                to: context.Message.PhoneNumber,
                body: "Notification Received via Twilio"
            );
        _logger.LogInformation("Consumed Event: {number}", context.Message.PhoneNumber);
        _logger.LogInformation("SMS Sent: {details}", messageDetails);
    }
}
