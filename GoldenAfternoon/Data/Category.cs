using System.ComponentModel.DataAnnotations;

namespace ChelsEsite.GoldenAfternoon.Data;

public sealed class Category
{
    [Key]
    public Guid Id { get; set; }

    [Required, MaxLength(100)]
    public required string Name { get; set; }

    public string? Description { get; set; }

    // One-to-Many with Product
    public ICollection<Product>? Products { get; set; }
}