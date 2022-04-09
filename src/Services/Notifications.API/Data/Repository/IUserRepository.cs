using Notifications.API.Models;

namespace Notifications.API.Data.Repository;

public interface IUserRepository
{
    Task AddAsync(User user, CancellationToken ct = default);
    void Delete(User user);
    Task<User?> GetUserById(int id, CancellationToken ct = default);
    User Update(User user);
    Task<int> CompleteChangesAsync(CancellationToken ct = default);
}
