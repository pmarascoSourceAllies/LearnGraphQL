using ChelsEsite.GoldenAfternoon.Data;
using ChelsEsite.GoldenAfternoon.Inputs;
using Microsoft.EntityFrameworkCore;

namespace ChelsEsite.GoldenAfternoon.Resolvers;
[QueryType]
public class PaymentQueryResolver(ApplicationDbContext dbContext)
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<Payment?> GetPayment(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.Payments.FindAsync(id, cancellationToken);
    }
}
