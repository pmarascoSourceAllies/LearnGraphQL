using System.ComponentModel.DataAnnotations;

namespace ChelsEsite.GoldenAfternoon.Data;

public sealed class Order
{
    [Key]
    public required Guid Id { get; init; }

    // Many-to-One with User (Customer)
    public required Guid UserID { get; init; }
    public required User User { get; init; } 

    public required string Status {get; set;}

    // One-to-Many with OrderItem
    public required ICollection<OrderItem> OrderItems { get; init; }

    public decimal TotalPrice => OrderItems.Sum(oi => oi.Price * oi.Quantity);

    // One-to-One with Payment
    public Payment? Payment { get; init; }

    [DataType(DataType.DateTime)]
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    [DataType(DataType.DateTime)]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}