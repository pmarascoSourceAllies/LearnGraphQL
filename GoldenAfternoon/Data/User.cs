using System.ComponentModel.DataAnnotations;

namespace ChelsEsite.GoldenAfternoon.Data;

public sealed class User
{
    [Key]
    public Guid Id { get; init; }

    [StringLength(200)]
    public required string Name { get; set; }

    [StringLength(320)] // Max length for email addresses
    [EmailAddress]
    public required string Email { get; set; }

    [DataType(DataType.Password)]
    public required string PasswordHash { get; set; }

    // One-to-One: Each User has ONE Role
    public Role Role { get; set; } // e.g., Admin, Customer

    // One-to-Many: Each User has Many Orders
    public ICollection<Order>? Orders { get; set; }


    [DataType(DataType.DateTime)]
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    [DataType(DataType.DateTime)]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}