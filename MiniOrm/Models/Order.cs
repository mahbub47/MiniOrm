using MiniOrm.MiniOrm.Library.Attributes;

namespace MiniOrm.Models;

[Table("MyOrders")]
public class Order
{
    [PrimaryKey]
    [Column(Name: "OrderId")]
    public int Id { get; set; }

    [Column]
    public int ProductId { get; set; }

    [Column]
    public int Quantity { get; set; }

    [Column]
    public DateTime? OrderDate { get; set; }
}
