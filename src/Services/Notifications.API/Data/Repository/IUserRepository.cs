using Notifications.API.Models;

namespace Notifications.API.Data.Repository;

public interface IUserRepository
{
    Task AddAsync(User user, CancellationToken ct);
    void Delete(User user);
    Task<User?> GetUserById(int id, CancellationToken ct);
    User Update(User user);
    Task<int> CompleteChangesAsync(CancellationToken ct);
}
