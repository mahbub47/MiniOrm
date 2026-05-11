
using MiniOrm.Models;

var context = new AppDbContext("Host=localhost;Username=postgres;Password=MyPGServer;Database=miniOrm");

var newProduct = new Product
{
    Name = "Test Product",
    Price = 500m,
    InStock = true
};

var result = await context.Products!.Insert(newProduct);

Console.WriteLine($"Id: {result}");