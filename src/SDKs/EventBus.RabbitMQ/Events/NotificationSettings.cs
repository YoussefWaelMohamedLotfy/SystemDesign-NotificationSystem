namespace EventBus.RabbitMQ.Events;

public interface NotificationSettings
{
    string Channel { get; }

    bool IsOptIn { get; }
}
