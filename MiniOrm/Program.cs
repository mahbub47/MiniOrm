
using MiniOrm.Models;

var connectionString = Environment.GetEnvironmentVariable("MINIORM_CONN");
var context = new AppDbContext(connectionString!);

var newProduct = new Product
{
    Name = "Test Product 1",
    Price = 500m,
    InStock = true
}
;

var result = await context.Products!.InsertAsync(newProduct);

//var result = await context.Products!.FindByIdAsync(1);

//result.Name = "Test Product Updated";

//Console.WriteLine($"Id: {result.Id}, ProductName: {result.Name}, Price: {result.Price}, Discount: {result.Discount}, IsAvailable: {result.InStock}");

//var result = await context.Products!.GetAllAsync();

//await context.Products!.UpdateAsync(result);

//await context.Products!.DeleteAsync(1);
