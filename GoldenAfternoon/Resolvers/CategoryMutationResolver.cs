using ChelsEsite.GoldenAfternoon.Data;
using ChelsEsite.GoldenAfternoon.Inputs;
using Microsoft.EntityFrameworkCore;

namespace ChelsEsite.GoldenAfternoon.Resolvers;

[MutationType]
public class CategoryMutationResolver
{
    private readonly ApplicationDbContext _dbContext;

    public CategoryMutationResolver(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Category> CreateCategory(CreateCategoryInput input, CancellationToken cancellationToken)
    {
        var category = new Category
        {
            Id = Guid.NewGuid(),
            Name = input.Name,
            Description = input.Description
        };

        _dbContext.Categories.Add(category);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return category;
    }
}
