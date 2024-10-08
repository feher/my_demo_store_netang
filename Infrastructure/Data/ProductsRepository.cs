using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class ProductsRepository(StoreContext context) : IProductsRepository
{
    public void CreateProduct(Product product)
    {
        context.Products.Add(product);
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await context.Products.FindAsync(id);
    }

    public async Task<IReadOnlyList<Product>> GetProductsAsync(string? brandName, string? typeName, string? sortType)
    {
        var query = context.Products.AsQueryable();
        if (!string.IsNullOrWhiteSpace(brandName))
        {
            query = query.Where(p => p.Brand == brandName);
        }
        if (!string.IsNullOrWhiteSpace(typeName))
        {
            query = query.Where(p => p.Type == typeName);
        }
        query = sortType switch
        {
            "priceAsc" => query.OrderBy(p => p.Price),
            "priceDesc" => query.OrderByDescending(p => p.Price),
            _ => query.OrderBy(p => p.Name)
        };
        return await query.ToListAsync();
    }

    public async Task<IReadOnlyList<string>> GetBrandsAsync()
    {
        return await context.Products
            .Select(product => product.Brand)
            .Distinct()
            .ToListAsync();
    }

    public async Task<IReadOnlyList<string>> GetTypesAsync()
    {
        return await context.Products
            .Select(product => product.Type)
            .Distinct()
            .ToListAsync();
    }

    public void UpdateProduct(Product product)
    {
        context.Products.Update(product);
    }

    public void DeleteProduct(Product product)
    {
        context.Products.Remove(product);
    }

    public bool DoesProductExist(int id)
    {
        return context.Products.Any(product => product.Id == id);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await context.SaveChangesAsync() != 0;
    }
}
