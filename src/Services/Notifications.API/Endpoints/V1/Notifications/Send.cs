using Ardalis.ApiEndpoints;
using EventBus.RabbitMQ.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Notifications.API.Data.Repository;

namespace Notifications.API.Endpoints.V1.Notifications;

public class Send : EndpointBaseAsync.WithRequest<int>.WithActionResult
{
    private readonly IUserRepository _userRepository;
    private readonly IPublishEndpoint _publishEndpoint;

    public Send(IPublishEndpoint publishEndpoint, IUserRepository userRepository)
    {
        _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    /// <summary>
    /// Sends a notification to User with ID
    /// </summary>
    /// <param name="userID"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <response code="204">Notification Sent to User</response>
    /// <response code="404">User Not Found</response>
    [HttpPost("/api/[namespace]/Send/{userID}")]
    public override async Task<ActionResult> HandleAsync(int userID, CancellationToken cancellationToken = default)
    {
        var userInDB = await _userRepository.GetUserById(userID, cancellationToken);

        if (userInDB is null)
            return NotFound();

        await _publishEndpoint.Publish<SendNotificationEvent>(new
        {
            userInDB.Name,
            userInDB.EmailAddress,
            userInDB.PhoneNumber,
            userInDB.Settings
        }, cancellationToken);

        return Accepted();
    }
}
