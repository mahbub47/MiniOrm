using MiniOrm.MiniOrm.Library.Attributes;

namespace MiniOrm.Models;

[Table]
public class Product
{
    [PrimaryKey]
    [Column(Name: "ProductId")]
    public int Id { get; set; }

    [Column(Name: "ProductName")]
    public string Name { get; set; } = string.Empty;

    [Column]
    public decimal Price { get; set; }

    [Column]
    public decimal? Discount { get; set; }

    [Column]
    public bool InStock { get; set; }
}
