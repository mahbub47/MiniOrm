using MiniOrm.Data;
using MiniOrm.Models;

/// <summary>
/// Represents the Entity Framework database context for the application, providing access to Product and Order
/// entities.
/// </summary>
/// <remarks>Use this context to query and save instances of Product and Order. The context manages the connection
/// to the underlying database and tracks changes to entities. This class should be configured with a valid connection
/// string to connect to the appropriate database.</remarks>
public class AppDbContext : DbContext
{
    public DbSet<Product>? Products { get; set; }
    public DbSet<Order>? Orders { get; set; }
    public AppDbContext(string connectionString) : base(connectionString) { }
}
