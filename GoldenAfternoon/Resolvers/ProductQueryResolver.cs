using ChelsEsite.GoldenAfternoon.Data;
using ChelsEsite.GoldenAfternoon.Inputs;
using Microsoft.EntityFrameworkCore;

namespace ChelsEsite.GoldenAfternoon.Resolvers;

[QueryType]
public class ProductQueryResolver
{
    private readonly ApplicationDbContext _dbContext;

    public ProductQueryResolver(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
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
        return await _dbContext.Products.FindAsync(id, cancellationToken);
    }
}
