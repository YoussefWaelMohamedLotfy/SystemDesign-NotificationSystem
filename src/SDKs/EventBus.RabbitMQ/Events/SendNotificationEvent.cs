namespace EventBus.RabbitMQ.Events;

public interface SendNotificationEvent
{
    string Name { get; }

    string EmailAddress { get; }

    string PhoneNumber { get; }

    NotificationSettings Settings { get; }
}
