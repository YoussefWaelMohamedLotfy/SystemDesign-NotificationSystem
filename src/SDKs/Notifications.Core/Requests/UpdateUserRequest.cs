namespace Notifications.Core.Requests;

public record UpdateUserRequest : CreateUserRequest
{
    public int ID { get; init; }
}
