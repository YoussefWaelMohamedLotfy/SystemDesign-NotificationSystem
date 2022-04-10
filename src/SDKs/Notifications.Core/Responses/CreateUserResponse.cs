using Notifications.Core.Requests;

namespace Notifications.Core.Responses;

public record CreateUserResponse : CreateUserRequest
{
    public int ID { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

}
