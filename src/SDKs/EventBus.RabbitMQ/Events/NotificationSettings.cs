namespace EventBus.RabbitMQ.Events;

public interface NotificationSettings
{
    NotifcationSetting Channel { get; }

    bool IsOptIn { get; }
}

public enum NotifcationSetting
{
    SMS,
    Email
}