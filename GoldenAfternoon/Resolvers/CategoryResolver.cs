using ChelsEsite.GoldenAfternoon.Data;
using Microsoft.EntityFrameworkCore;

public class CategoryResolver
{
    private readonly ApplicationDbContext _dbContext;

    public CategoryResolver(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Category>> GetCategorys(CancellationToken cancellationToken)
    {
        var query = _dbContext.Categories.AsQueryable();

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<Category?> GetCategory(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.Categories.FindAsync(id, cancellationToken);
    }
}
