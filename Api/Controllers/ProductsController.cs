using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")] // Will become "api/products".
public class ProductsController(IGenericRepository<Product> repository) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(string? brand, string? type, string? sort)
    {
        var products = await repository.GetAllBySpecAsync(new ProductSpecification<Product>(
            brandName: brand, typeName: type, sortType: sort));
        return new ActionResult<IReadOnlyList<Product>>(products);
    }

    [HttpGet("{id:int}")] // api/products/123
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await repository.GetByIdAsync(id);
        return (product is not null) ? product : NotFound();
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
    {
        var brands = await repository.GetAllBySpecAsync(new ProductSpecification<string>(selectField: "brand", distinct: true));
        return new ActionResult<IReadOnlyList<string>>(brands);
    }

    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
    {
        var types = await repository.GetAllBySpecAsync(new ProductSpecification<string>(selectField: "type", distinct: true));
        return new ActionResult<IReadOnlyList<string>>(types);
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        repository.Create(product);
        if (await repository.SaveChangesAsync())
        {
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }
        return BadRequest("Cannot create product.");
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateProduct(int id, Product product)
    {
        if (product.Id != id)
        {
            return BadRequest("ID mismatch.");
        }
        if (!repository.DoesExist(id))
        {
            return NotFound();
        }
        repository.Update(product);
        if (await repository.SaveChangesAsync())
        {
            return NoContent();
        }
        return BadRequest("Cannot update product.");
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var product = await repository.GetByIdAsync(id);
        if (product is null)
        {
            return NotFound();
        }
        repository.Delete(product);
        if (await repository.SaveChangesAsync())
        {
            return NoContent();
        }
        return BadRequest("Cannot delete product.");
    }
}
