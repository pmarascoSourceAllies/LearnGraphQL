using ChelsEsite.GoldenAfternoon.Data;
using ChelsEsite.GoldenAfternoon.Inputs;
using Microsoft.EntityFrameworkCore;

namespace ChelsEsite.GoldenAfternoon.Resolvers;
[MutationType]
public class OrderMutationResolver(ApplicationDbContext dbContext)
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<Order> CreateOrder(CreateOrderInput input, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.FindAsync(input.UserId, cancellationToken);
        if (user == null) throw new Exception("User not found.");
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
        var order = await _dbContext.Orders.FindAsync(id);
        if (order == null) throw new Exception("Order not found.");
        order.Status = status;
        await _dbContext.SaveChangesAsync(cancellationToken);
        return order;
    }
}