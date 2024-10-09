using Api.RequestHelpers;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class ProductsController(IGenericRepository<Product> repository) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<Pagination<Product>>> GetProducts([FromQuery] ProductParams paramz)
    {
        var specParams = paramz.ToSpecParams();
        var spec = new ProductSpecification<Product>(specParams);
        var result = await CreatePagedResultAsync(repository, spec, specParams.PageIndex, specParams.PageSize);
        return result;
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
        var specParams = new ProductSpecParams()
        {
            SelectField = nameof(Product.Brand),
            IsSelectionDistinct = true
        };
        var brands = await repository.GetAllBySpecAsync(new ProductSpecification<string>(specParams));
        return new ActionResult<IReadOnlyList<string>>(brands);
    }

    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
    {
        var specParams = new ProductSpecParams()
        {
            SelectField = nameof(Product.Type),
            IsSelectionDistinct = true
        };
        var types = await repository.GetAllBySpecAsync(new ProductSpecification<string>(specParams));
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
