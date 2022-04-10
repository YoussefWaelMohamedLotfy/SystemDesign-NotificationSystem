namespace Notifications.Core.Requests;

public record CreateNotificationSettingRequest
{
    public string Channel { get; init; } = default!;

    public bool IsOptIn { get; init; }
}
