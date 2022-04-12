using Ardalis.ApiEndpoints;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Notifications.API.Data.Repository;
using Notifications.Core.Requests;
using Notifications.Core.Responses;

namespace Notifications.API.Endpoints.V1.Users;

public class Update : EndpointBaseAsync.WithRequest<UpdateUserRequest>.WithActionResult<UpdateUserResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public Update(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <summary>
    /// Update an existing User
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <response code="200">User Updated Success</response>
    /// <response code="400">User Object has errors</response>
    /// <response code="404">User Not Found</response>
    [HttpPut("/api/[namespace]")]
    public override async Task<ActionResult<UpdateUserResponse>> HandleAsync([FromBody] UpdateUserRequest request, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userInDb = await _userRepository.GetUserById(request.ID, cancellationToken);

        if (userInDb is null)
            return NotFound();

        _mapper.Map(request, userInDb);
        await _userRepository.CompleteChangesAsync(cancellationToken);

        var response = _mapper.Map<UpdateUserResponse>(userInDb);
        return Ok(response);
    }
}
