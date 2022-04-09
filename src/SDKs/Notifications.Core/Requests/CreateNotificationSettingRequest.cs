namespace Notifications.Core.Requests;

public record CreateNotificationSettingRequest
{
    public string Channel { get; set; } = default!;

    public bool IsOptIn { get; set; }
}
