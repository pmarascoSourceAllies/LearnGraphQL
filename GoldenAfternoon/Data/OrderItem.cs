using System.ComponentModel.DataAnnotations;

namespace ChelsEsite.GoldenAfternoon.Data;

public sealed class OrderItem
{
    // Composite Primary Key (OrderId, ProductId)
    public required Guid OrderId { get; init; }
    public required Order Order { get; init; }

    public required Guid ProductId { get; init; }
    public required Product Product { get; init; }

    [Required]
    public required int Quantity { get; init; }

    [Required]
    public decimal Price => Product.Price * Quantity;
}