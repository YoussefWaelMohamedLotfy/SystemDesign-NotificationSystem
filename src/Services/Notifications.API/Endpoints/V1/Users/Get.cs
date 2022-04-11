using Ardalis.ApiEndpoints;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Notifications.API.Data.Repository;
using Notifications.Core.Responses;

namespace Notifications.API.Endpoints.V1.Users
{
    public class Get : EndpointBaseAsync.WithRequest<int>.WithActionResult
    {
        private readonly IUserRepository _repo;
        private readonly IMapper _mapper;

        public Get(IUserRepository repo, IMapper mapper)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet("/api/[namespace]/{id}", Name = "[namespace]_[controller]")]
        public override async Task<ActionResult> HandleAsync(int id, CancellationToken cancellationToken = default)
        {
            var user = await _repo.GetUserById(id, cancellationToken);

            if (user is null)
                return NotFound();

            var response = _mapper.Map<GetUserResponse>(user);
            return Ok(response);
        }
    }
}
