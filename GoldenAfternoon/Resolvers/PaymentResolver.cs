using ChelsEsite.GoldenAfternoon.Data;
using ChelsEsite.GoldenAfternoon.Inputs;
using Microsoft.EntityFrameworkCore;

public class PaymentResolver
{
    private readonly ApplicationDbContext _dbContext;

    public PaymentResolver(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [Query]
    public async Task<Payment?> GetPayment(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.Payments.FindAsync(id, cancellationToken);
    }

    [Mutation]
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
}
