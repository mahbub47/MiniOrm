using System;
using System.Collections.Generic;
using System.Text;

namespace MiniOrm.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal? Discount { get; set; }
    public bool InStock { get; set; }
}
