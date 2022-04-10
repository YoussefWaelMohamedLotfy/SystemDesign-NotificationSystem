using Notifications.Core.Requests;

namespace Notifications.Core.Responses;

public record UpdateUserResponse : CreateUserRequest
{
    public int ID { get; init; }
}
