namespace Notifications.API.Models;

public class NotificationSetting
{
    public int UserID { get; set; }

    public NotificationChannel Channel { get; set; }

    public bool IsOptIn { get; set; }

    public User? User { get; set; }
}

public enum NotificationChannel
{ 
    SMS,
    Email
}
