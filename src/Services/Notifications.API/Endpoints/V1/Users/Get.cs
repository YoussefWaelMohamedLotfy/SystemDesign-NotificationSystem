using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Notifications.API.Data.Repository;

namespace Notifications.API.Endpoints.V1.Users
{
    public class Get : EndpointBaseAsync.WithRequest<int>.WithActionResult
    {
        private readonly IUserRepository _repo;

        public Get(IUserRepository repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        [HttpGet("/api/[namespace]/{id}")]
        public override async Task<ActionResult> HandleAsync(int id, CancellationToken cancellationToken = default)
        {
            var user = await _repo.GetUserById(id, cancellationToken);

            if (user is null)
                return NotFound();

            return Ok(user);
        }
    }
}
