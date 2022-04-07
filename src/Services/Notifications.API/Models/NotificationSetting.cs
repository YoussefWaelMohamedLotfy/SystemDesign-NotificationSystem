namespace Notifications.API.Models;

public class NotificationSetting
{
    public int UserID { get; set; }

    public string Channel { get; set; } = default!;

    public bool IsOptIn { get; set; }

    public User? User { get; set; }
}
