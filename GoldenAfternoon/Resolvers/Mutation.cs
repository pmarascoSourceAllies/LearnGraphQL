using ChelsEsite.GoldenAfternoon.Data;
using ChelsEsite.GoldenAfternoon.Inputs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ChelsEsite.GoldenAfternoon.Resolvers;

[MutationType]
public class Mutation(ApplicationDbContext dbContext)
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    private PasswordHasher<string> passwordHasher = new PasswordHasher<string>();

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

    public async Task<Category> UpdateCategory(Guid id, UpdateCategoryInput input, CancellationToken cancellationToken)
    {
        var category = await _dbContext.Categories.FindAsync([id], cancellationToken: cancellationToken) ?? throw new Exception("Product not found.");

        category.Description = input.Description ?? category.Description;
        if (input.ProductIDs != null && input.ProductIDs.Count > 0)
        {
            var products = await _dbContext.Products
                .Where(p => input.ProductIDs.Contains(p.Id))
                .ToListAsync(cancellationToken);

            if (products.Count != input.ProductIDs.Count)
            {
                throw new Exception("One or more products not found.");
            }

            category.Products = products;
        }

        category.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);
        return category;
    }

    public async Task<Category> AddProductToCategory(Guid id, AddProductToCategoryInput input, CancellationToken cancellationToken)
    {
        var category = await _dbContext.Categories
            .Include(c => c.Products)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        if (category == null)
            throw new Exception("Category not found.");

        if (input.ProductIDs?.Any() != true)
            throw new Exception("No valid product IDs provided.");

        var products = await _dbContext.Products
            .Where(p => input.ProductIDs.Contains(p.Id))
            .ToListAsync(cancellationToken);

        if (products.Count != input.ProductIDs.Count)
            throw new Exception("One or more products not found.");

        // Add new products to category
        category.Products ??= [];
        foreach (var product in products)
        {
            if (!category.Products.Contains(product))
            {
                category.Products.Add(product);
            }
        }
        category.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);
        return category;
    }

    public async Task<bool> DeleteCategory(Guid id, CancellationToken cancellationToken)
    {
        var category = await _dbContext.Categories.FindAsync([id], cancellationToken: cancellationToken);
        if (category == null) return false;

        _dbContext.Categories.Remove(category);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<Order> CreateOrder(CreateOrderInput input, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.FindAsync([input.UserId], cancellationToken) ?? throw new Exception("User not found.");
        var productIds = input.Items.Select(i => i.ProductId).ToList();
        var products = await _dbContext.Products
                                    .Where(p => productIds.Contains(p.Id))
                                    .ToListAsync(cancellationToken);
        var oid = Guid.NewGuid();
        var order = new Order
        {
            Id = oid,
            User = user,
            UserID = input.UserId,
            Status = "Pending",
            OrderItems = new HashSet<OrderItem>()
        };
        // ✅ Create OrderItems and link them to Order & Products
        foreach (var item in input.Items)
        {
            var product = products.First(p => p.Id == item.ProductId);

            var orderItem = new OrderItem
            {
                Id = Guid.NewGuid(),
                OrderId = order.Id,
                Order = order,  // ✅ Link Order
                ProductId = product.Id,
                Product = product,  // ✅ Include full Product object
                Quantity = item.Quantity
            };

            order.OrderItems.Add(orderItem);
        }

        _dbContext.Orders.Add(order);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return order;
    }

    public async Task<Order> UpdateOrderStatus(Guid id, String status, CancellationToken cancellationToken)
    {
        var order = await _dbContext.Orders.FindAsync([id], cancellationToken: cancellationToken) ?? throw new Exception("Order not found.");
        order.Status = status;
        await _dbContext.SaveChangesAsync(cancellationToken);
        return order;
    }

    public async Task<Payment> CreatePayment(CreatePaymentInput input, CancellationToken cancellationToken)
    {
        if (await _dbContext.Payments.AnyAsync(p => p.Order.Id == input.OrderId))
            throw new Exception("Email already exists.");

        var order = await _dbContext.Orders.FindAsync(input.OrderId);
        if (order == null) throw new Exception("Category not found.");

        var payment = new Payment
        {
            Id = Guid.NewGuid(),
            Order = order,
            OrderId = input.OrderId,
            Amount = input.Amount,
            Method = input.Method,
            Status = "Created"
        };

        _dbContext.Payments.Add(payment);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return payment;
    }

    public async Task<Product> CreateProduct(CreateProductInput input, CancellationToken cancellationToken)
    {
        if (await _dbContext.Products.AnyAsync(p => p.Name == input.Name, cancellationToken: cancellationToken))
            throw new Exception("Product already exists.");

        var category = await _dbContext.Categories.FindAsync([input.CategoryID], cancellationToken: cancellationToken) ?? throw new Exception("Category not found.");
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

    public async Task<Product> UpdateProduct(Guid id, UpdateProductInput input, CancellationToken cancellationToken)
    {
        var product = await _dbContext.Products.FindAsync([id], cancellationToken: cancellationToken) ?? throw new Exception("Product not found.");
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

    public async Task<bool> DeleteProduct(Guid id, CancellationToken cancellationToken)
    {
        var product = await _dbContext.Products.FindAsync([id], cancellationToken: cancellationToken);
        if (product == null) return false;

        _dbContext.Products.Remove(product);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
    public async Task<User> CreateUser(CreateUserInput input, CancellationToken cancellationToken)
    {
        if (await _dbContext.Users.AnyAsync(u => u.Email == input.Email))
            throw new Exception("Email already exists.");


        var hashedPassword = passwordHasher.HashPassword(input.Email, input.Password);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = input.Name,
            Email = input.Email,
            Role = input.Role,
            PasswordHash = hashedPassword
        };

        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return user;
    }
    public async Task<User> UpdateUser(Guid id, UpdateUserInput input, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.FindAsync([id], cancellationToken: cancellationToken) ?? throw new Exception("User not found.");
        user.Name = input.Name ?? user.Name;
        if (input.Email != null)
        {
            if (await _dbContext.Users.AnyAsync(u => u.Email == input.Email))
                throw new Exception("Email already exists.");
            user.Email = input.Email ?? user.Email;
        }
        if (input.Password != null)
        {
            user.PasswordHash = passwordHasher.HashPassword(user.Email, input.Password);
        }
        user.Role = input.Role ?? user.Role;

        await _dbContext.SaveChangesAsync(cancellationToken);
        return user;
    }

    public async Task<bool> DeleteUser(Guid id, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.FindAsync([id], cancellationToken);
        if (user == null) return false;

        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}
