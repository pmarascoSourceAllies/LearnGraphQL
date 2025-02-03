using ChelsEsite.GoldenAfternoon.Data;
using ChelsEsite.GoldenAfternoon.Inputs;
using Microsoft.EntityFrameworkCore;

namespace ChelsEsite.GoldenAfternoon.Resolvers;
[QueryType]
public class OrderQueryResolver(ApplicationDbContext dbContext)
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    [Query]
    public async Task<IEnumerable<Order>> GetOrders(Guid? userId, CancellationToken cancellationToken)
    {
        var query = _dbContext.Orders.AsQueryable();

        if (userId.HasValue)
            query = query.Where(p => p.UserID == userId);

        return await query.ToListAsync(cancellationToken);
    }

    [Query]
    public async Task<Order?> GetOrder(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.Orders.FindAsync(id, cancellationToken);
    }
}
