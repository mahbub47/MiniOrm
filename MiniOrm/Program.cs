
var context = new AppDbContext("Host=localhost;Username=postgres;Password=MyPGServer;Database=miniOrm");
var products = context.Products!.ToList();
foreach (var product in products)
{
    Console.WriteLine($"Name: {product.Name}");
}
