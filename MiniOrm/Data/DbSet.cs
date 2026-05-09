using MiniOrm.Models;

namespace MiniOrm.Data;

/// <summary>
/// This class represents a set of entities of a specific type, providing methods to query and manipulate the data.
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <param name="context"></param>
public class DbSet<TEntity>(DbContext context) where TEntity : class
{
    public IEnumerable<Product> ToList()
    {
        return new List<Product>()
        {
            new Product{ Id = 1, Name = "Test", Price = 10m, Discount = 10m, InStock = true}
        };
    }
}