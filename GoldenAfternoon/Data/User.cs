using System.ComponentModel.DataAnnotations;

namespace ChelsEsite.GoldenAfternoon.Data;

public sealed class User
{
    public int Id { get; init; }

    [StringLength(200)]
    public required string Name { get; init; }

    [StringLength(320)] // Max length for email addresses
    [EmailAddress]
    public required string Email { get; init; }

    [StringLength(100)]
    public required string Role { get; init; } // e.g., Admin, Customer

    [DataType(DataType.Password)]
    public required string PasswordHash { get; init; }
    // public required string Password { get; init; }

    [DataType(DataType.DateTime)]
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    [DataType(DataType.DateTime)]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}