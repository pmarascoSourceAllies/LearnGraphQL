using System.ComponentModel.DataAnnotations;

namespace ChelsEsite.GoldenAfternoon.Data;

public sealed class Product{
    [Key]
    public Guid Id { get; init; }

    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;
    public string? Description { get; internal set; }

    public required decimal Price { get; set; }

    public required int Stock { get; set; }

    public required string ImageUrl { get; set; }

    // Many-to-One with Category
    public required Guid CategoryId { get; set; } //used to pair
    public  required Category Category { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    [DataType(DataType.DateTime)]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}