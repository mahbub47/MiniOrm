using MiniOrm.Data;
using MiniOrm.Models;

public class AppDbContext : DbContext
{
    public DbSet<Product>? Products { get; set; }
    public DbSet<Order>? Orders { get; set; }
    public AppDbContext(string connectionString) : base(connectionString)
    {
        Products = Set<Product>();
        Orders = Set<Order>();
    }
}
