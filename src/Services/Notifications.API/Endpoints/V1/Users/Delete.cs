using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Notifications.API.Data.Repository;

namespace Notifications.API.Endpoints.V1.Users;

public class Delete : EndpointBaseAsync.WithRequest<int>.WithActionResult
{
    private readonly IUserRepository _userRepository;

    public Delete(IUserRepository userRepository)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    [HttpDelete("/api/[namespace]/{id}")]
    public override async Task<ActionResult> HandleAsync(int id, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetUserById(id, cancellationToken);

        if (user is null)
            return NotFound();

        _userRepository.Delete(user);
        await _userRepository.CompleteChangesAsync(cancellationToken);

        return NoContent();
    }
}
