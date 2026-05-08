using MiniOrm.Models;

namespace MiniOrm.Data;

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