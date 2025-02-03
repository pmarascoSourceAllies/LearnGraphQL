using ChelsEsite.GoldenAfternoon.Data;
using Microsoft.EntityFrameworkCore;

namespace ChelsEsite.GoldenAfternoon.Resolvers;
[QueryType]
public class Query(ApplicationDbContext dbContext)
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<IEnumerable<Category>> GetCategories(CancellationToken cancellationToken)
    {
        var query = _dbContext.Categories.AsQueryable();

        return await query.ToListAsync(cancellationToken);
    }
    public async Task<Category?> GetCategory(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.Categories.FindAsync([id], cancellationToken: cancellationToken);
    }
    public async Task<IEnumerable<Order>> GetOrders(Guid? userId, CancellationToken cancellationToken)
    {
        var query = _dbContext.Orders.AsQueryable();

        if (userId.HasValue)
            query = query.Where(p => p.UserID == userId);

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<Order?> GetOrder(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.Orders.FindAsync([id], cancellationToken: cancellationToken);
    }

    public async Task<Payment?> GetPayment(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.Payments.FindAsync([id], cancellationToken: cancellationToken);
    }

    public async Task<IEnumerable<Product>> GetProducts(Guid? categoryId, string? search, CancellationToken cancellationToken)
    {
        var query = _dbContext.Products.AsQueryable();

        if (categoryId.HasValue)
            query = query.Where(p => p.CategoryId == categoryId);

        if (!string.IsNullOrEmpty(search))
            query = query.Where(p => p.Name.Contains(search));

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<Product?> GetProduct(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.Products.FindAsync([id], cancellationToken: cancellationToken);
    }

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
        return await _dbContext.Users.FindAsync([id], cancellationToken: cancellationToken);
    }
}
