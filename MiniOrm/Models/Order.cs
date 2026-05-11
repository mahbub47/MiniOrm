using MiniOrm.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

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

    [Column]
    public string? ShippingAddress { get; set; }
}
