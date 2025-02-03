using ChelsEsite.GoldenAfternoon.Data;
using Microsoft.EntityFrameworkCore;

namespace ChelsEsite.GoldenAfternoon.Resolvers;

[QueryType]
public class UserQueryResolver(ApplicationDbContext dbContext)
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<IEnumerable<User>> GetUsers(string? search, Role? role, CancellationToken cancellationToken)
    {
        var query = _dbContext.Users.AsQueryable();

        if (!string.IsNullOrEmpty(search))
            query = query.Where(u => u.Name.Contains(search));

        if (role != null)
            query = query.Where(u => u.Role == role);

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<User?> GetUser(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.Users.FindAsync(id, cancellationToken);
    }
}