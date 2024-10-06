using System.Text.Json;
using Core.Entities;

namespace Infrastructure.Data;

public class StoreContextSeed
{
    public static async Task SeedDataAsync(StoreContext context)
    {
        if (context.Products.Any())
        {
            return;
        }
        // The current directory is the Api directory (the startup project).
        var productsJson = File.OpenRead("../Infrastructure/Data/SeedData/products.json");
        var products = await JsonSerializer.DeserializeAsync<List<Product>>(productsJson);
        if (products is null)
        {
            return;
        }
        await context.Products.AddRangeAsync(products);
        await context.SaveChangesAsync();
    }
}
