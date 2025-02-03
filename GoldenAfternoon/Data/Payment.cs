using System.ComponentModel.DataAnnotations;

namespace ChelsEsite.GoldenAfternoon.Data;

public sealed class Payment
{
    [Key]
    public Guid Id { get; set; }

    // One-to-One with Order
    public required Guid OrderId { get; init; }
    public required Order Order { get; init; }

    [Required]
    public required decimal Amount { get; init; }

    public required string Method { get; init; }

    [Required, MaxLength(50)]
    public required string Status { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    [DataType(DataType.DateTime)]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
