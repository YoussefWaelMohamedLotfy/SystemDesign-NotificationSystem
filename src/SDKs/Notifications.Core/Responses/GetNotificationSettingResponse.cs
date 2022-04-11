namespace Notifications.Core.Responses;

public record GetNotificationSettingResponse
{
    public string Channel { get; set; } = default!;

    public bool IsOptIn { get; set; }
}
