using Ardalis.ApiEndpoints;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Notifications.API.Data.Repository;
using Notifications.API.Models;
using Notifications.Core.Requests;
using Notifications.Core.Responses;

namespace Notifications.API.Endpoints.V1.Users
{
    public class Post : EndpointBaseAsync.WithRequest<CreateUserRequest>.WithActionResult<CreateUserResponse>
    {
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;

        public Post(IUserRepository userRepo, IMapper mapper)
        {
            _userRepo = userRepo ?? throw new ArgumentNullException(nameof(userRepo));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }


        [HttpPost("/api/[namespace]")]
        public override async Task<ActionResult<CreateUserResponse>> HandleAsync(CreateUserRequest request, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var newUser = _mapper.Map<User>(request);
            newUser.CreatedAt = DateTimeOffset.UtcNow;

            await _userRepo.AddAsync(newUser, cancellationToken);
            await _userRepo.CompleteChangesAsync(cancellationToken);

            var response = _mapper.Map<CreateUserResponse>(newUser);
            return CreatedAtRoute("Users_Get", new { id = response.ID }, response);
        }
    }
}
