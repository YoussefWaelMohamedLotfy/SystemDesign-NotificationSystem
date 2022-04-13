using Notifications.Core.Requests;
using Notifications.Core.Responses;
using Refit;

namespace Notifications.UI.RefitHttpClients;

public interface IUsersApi
{
    [Get("/api/Users/{id}")]
    Task<ApiResponse<GetUserResponse>> GetUserById(int id);

    [Post("/api/Users")]
    Task<ApiResponse<CreateUserResponse>> CreateNewUser([Body]CreateUserRequest newUser);

    [Put("/api/Users")]
    Task<ApiResponse<CreateUserResponse>> UpdateUser([Body] UpdateUserRequest updatedUser);
    
    [Delete("/api/Users/{id}")]
    Task<ApiResponse<CreateUserResponse>> DeleteUser(int id);

}
