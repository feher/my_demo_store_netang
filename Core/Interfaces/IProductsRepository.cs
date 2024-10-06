using Core.Entities;

namespace Core.Interfaces;

public interface IProductsRepository
{
    void CreateProduct(Product product);
    Task<IReadOnlyList<Product>> GetProductsAsync(string? brandName, string? typeName, string? sortType);
    Task<Product?> GetProductByIdAsync(int id);
    Task<IReadOnlyList<string>> GetBrandsAsync();
    Task<IReadOnlyList<string>> GetTypesAsync();
    void UpdateProduct(Product product);
    void DeleteProduct(Product id);
    bool DoesProductExist(int id);

    Task<bool> SaveChangesAsync();
}
