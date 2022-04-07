namespace Notifications.API.Models;

public class User
{
    public int ID { get; set; }

    public string Name { get; set; } = default!;

    public string EmailAddress { get; set; } = default!;

    public string PhoneNumber { get; set; } = default!;

    public DateTimeOffset CreatedAt { get; set; }

    public NotificationSetting? Settings { get; set; }
}
