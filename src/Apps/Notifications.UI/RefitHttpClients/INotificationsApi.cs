using Refit;

namespace Notifications.UI.RefitHttpClients;

public interface INotificationsApi
{
    [Post("/api/Notifications/Send/{userId}")]
    Task SendNotificationToUserId(int userId);
}
