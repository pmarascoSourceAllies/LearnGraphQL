using Microsoft.EntityFrameworkCore;

namespace ChelsEsite.GoldenAfternoon.Data;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Payment> Payments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // One-to-Many: A Role can have many Users, but a User has one Role
        modelBuilder.Entity<User>()
            .Property(u => u.Role)
            .HasConversion<string>(); // Stores enum as a string

        // One-to-Many (Category <-> Product)
        modelBuilder.Entity<Product>()
            .HasOne(p => p.Category) //One Category
            .WithMany(c => c.Products) // Has many Products
            .HasForeignKey(p => p.CategoryId); // Use Category Pair

        // One-to-Many (User <-> Order)
        modelBuilder.Entity<Order>()
            .HasOne(o => o.User) // One User
            .WithMany(u => u.Orders) // Has many orders
            .HasForeignKey(o => o.UserID); // Use UserID Pair

        // One-to-Many (Order <-> OrderItem)
        modelBuilder.Entity<OrderItem>()
            .HasKey(oi => new { oi.OrderId, oi.ProductId });

        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Order)
            .WithMany(o => o.OrderItems)
            .HasForeignKey(oi => oi.OrderId);

        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Product)
            .WithMany()
            .HasForeignKey(oi => oi.ProductId);

        // One-to-One (Order <-> Payment)
        modelBuilder.Entity<Payment>()
            .HasOne(p => p.Order)
            .WithOne(o => o.Payment)
            .HasForeignKey<Payment>(p => p.OrderId);
    }

}