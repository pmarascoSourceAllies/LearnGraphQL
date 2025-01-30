using ChelsEsite.GoldenAfternoon.Data;
using ChelsEsite.GoldenAfternoon.Inputs;
using Microsoft.EntityFrameworkCore;

public class ProductResolver
{
    private readonly ApplicationDbContext _dbContext;

    public ProductResolver(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [Query]
    public async Task<IEnumerable<Product>> GetProducts(Guid? categoryId, string? search, CancellationToken cancellationToken)
    {
        var query = _dbContext.Products.AsQueryable();

        if (categoryId.HasValue)
            query = query.Where(p => p.CategoryId == categoryId);

        if (!string.IsNullOrEmpty(search))
            query = query.Where(p => p.Name.Contains(search));

        return await query.ToListAsync(cancellationToken);
    }

    [Query]
    public async Task<Product?> GetProduct(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.Products.FindAsync(id, cancellationToken);
    }

    [Mutation]
    public async Task<Product> CreateProduct(CreateProductInput input, CancellationToken cancellationToken)
    {
        if (await _dbContext.Products.AnyAsync(p => p.Name == input.Name))
            throw new Exception("Product already exists.");

        var category = await _dbContext.Categories.FindAsync(input.CategoryID);
        if (category == null) throw new Exception("Category not found.");

        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = input.Name,
            Description = input.Description,
            Price = input.Price,
            Stock = input.Stock,
            ImageUrl = input.ImageUrl,
            Category = category,
            CategoryId = input.CategoryID
        };

        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return product;
    }

    [Mutation]
    public async Task<Product> UpdateProduct(Guid id, UpdateProductInput input, CancellationToken cancellationToken)
    {
        var product = await _dbContext.Products.FindAsync(id);
        if (product == null) throw new Exception("Product not found.");

        if (input.Name != null)
        {
            if (await _dbContext.Products.AnyAsync(p => p.Name == input.Name))
                throw new Exception("Name already exists for different Product.");
            product.Name = input.Name;
        }

        product.Description = input.Description ?? product.Description;
        product.Price = input.Price ?? product.Price;
        product.Stock = input.Stock ?? product.Stock;
        product.ImageUrl = input.ImageUrl ?? product.ImageUrl;

        await _dbContext.SaveChangesAsync(cancellationToken);
        return product;
    }

    [Mutation]
    public async Task<bool> DeleteProduct(Guid id, CancellationToken cancellationToken)
    {
        var product = await _dbContext.Products.FindAsync(id);
        if (product == null) return false;

        _dbContext.Products.Remove(product);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}
