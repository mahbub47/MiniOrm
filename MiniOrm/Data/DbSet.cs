using MiniOrm.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

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

    //public int Id { get; set; }
    //public string Name { get; set; } = string.Empty;
    //public decimal Price { get; set; }
    //public decimal? Discount { get; set; }
    //public bool InStock { get; set; }
