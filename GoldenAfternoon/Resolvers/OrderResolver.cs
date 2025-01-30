using ChelsEsite.GoldenAfternoon.Data;
using Microsoft.EntityFrameworkCore;

public class OrderResolver
{
    private readonly ApplicationDbContext _dbContext;

    public OrderResolver(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

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
