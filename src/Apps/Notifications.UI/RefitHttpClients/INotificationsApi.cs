using Refit;

namespace Notifications.UI.RefitHttpClients;

public interface INotificationsApi
{
    [Post("/api/Notifications/{userId}")]
    Task SendNotificationToUserId(int userId);
}
