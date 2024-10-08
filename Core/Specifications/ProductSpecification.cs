using Core.Entities;

namespace Core.Specifications;

public class ProductSpecification<TResult> : BaseSpecification<Product, TResult>
where TResult : class
{
    public ProductSpecification(string? brandName = null, string? typeName = null, string? sortType = null, string? selectField = null, bool distinct = true)
    : base(product => (string.IsNullOrWhiteSpace(brandName) || product.Brand == brandName) &&
                      (string.IsNullOrWhiteSpace(typeName) || product.Type == typeName))
    {
        switch (sortType)
        {
            case "priceAsc":
                OrderBy = product => product.Price;
                break;
            case "priceDesc":
                OrderByDescending = product => product.Price;
                break;
            default:
                OrderBy = product => product.Name;
                break;
        }

#pragma warning disable CS8603 // Possible null reference return.
        switch (selectField)
        {
            case "brand":
                Select = product => product.Brand as TResult;
                break;
            case "type":
                Select = product => product.Type as TResult;
                break;
        }
#pragma warning restore CS8603 // Possible null reference return.

        Distinct = distinct;
    }
}
