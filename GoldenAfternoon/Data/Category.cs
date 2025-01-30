using System.ComponentModel.DataAnnotations;

namespace ChelsEsite.GoldenAfternoon.Data;

public sealed class Category
{
    [Key]
    public Guid Id { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    // One-to-Many with Product
    public ICollection<Product>? Products { get; set; }
}