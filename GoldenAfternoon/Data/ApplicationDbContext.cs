using Microsoft.EntityFrameworkCore;

namespace ChelsEsite.GoldenAfternoon.Data;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options)
{
    public DbSet<User> Users { get; init; }
}