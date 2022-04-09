using Microsoft.EntityFrameworkCore;
using Notifications.API.Models;

namespace Notifications.API.Data.Repository;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    private readonly DbSet<User> _db;

    public UserRepository(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _db = _context.Set<User>();
    }

    public async Task<User?> GetUserById(int id, CancellationToken ct = default) 
        => await _db.Include(u => u.Settings).SingleOrDefaultAsync(u => u.ID == id, ct);

    public async Task AddAsync(User user, CancellationToken ct = default) 
        => await _db.AddAsync(user, ct);

    public User Update(User user)
    {
        //_db.Attach(user);
        //_context.Entry(user).State = EntityState.Modified;
        _db.Update(user);
        return user;
    }

    public void Delete(User user) => _db.Remove(user);

    public async Task<int> CompleteChangesAsync(CancellationToken ct = default)
        => await _context.SaveChangesAsync(ct);
}
