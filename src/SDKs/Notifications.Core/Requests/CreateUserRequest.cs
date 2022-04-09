namespace Notifications.Core.Requests;

public record CreateUserRequest
{
    public string Name { get; init; } = default!;

    public string EmailAddress { get; init; } = default!;

    public string PhoneNumber { get; init; } = default!;

    public CreateNotificationSettingRequest Settings { get; init; } = default!;
}
