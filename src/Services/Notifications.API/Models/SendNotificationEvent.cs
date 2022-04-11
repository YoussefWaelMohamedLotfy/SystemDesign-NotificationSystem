namespace Notifications.API.Models;

public interface SendNotificationEvent
{
    string Name { get; }

    string EmailAddress { get; }
    
    string PhoneNumber { get; }
}
