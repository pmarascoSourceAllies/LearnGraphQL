using ChelsEsite.GoldenAfternoon.Data;
using Microsoft.EntityFrameworkCore;

namespace ChelsEsite.GoldenAfternoon;

public static class Queries
{
    [Query]
    public static async Task<IEnumerable<User>> GetUsersAsync(
        ApplicationDbContext dbContext,
        CancellationToken cancellationToken)
    {
        return await dbContext.Users.AsNoTracking().ToListAsync(cancellationToken);
    }
}