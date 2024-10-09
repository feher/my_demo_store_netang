using Core.Entities;

namespace Core.Specifications;

public class ProductSpecification<TResult> : BaseSpecification<Product, TResult>
where TResult : class
{
    public ProductSpecification(ProductSpecParams specParams)
    : base(product => (string.IsNullOrWhiteSpace(specParams.SearchTerm) || product.Name.ToLower().Contains(specParams.SearchTerm)) &&
                      (specParams.Brands.Count == 0 || specParams.Brands.Contains(product.Brand)) &&
                      (specParams.Types.Count == 0 || specParams.Types.Contains(product.Type)))
    {
        switch (specParams.SortBy)
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

        if (!string.IsNullOrWhiteSpace(specParams.SelectField))
        {
#pragma warning disable CS8603 // Possible null reference return.
            switch (specParams.SelectField)
            {
                case nameof(Product.Brand):
                    Select = product => product.Brand as TResult;
                    break;
                case nameof(Product.Type):
                    Select = product => product.Type as TResult;
                    break;
            }
            Distinct = specParams.IsSelectionDistinct;
#pragma warning restore CS8603 // Possible null reference return.
        }

        IsPagingEnabled = specParams.IsPagingEnabled;
        Skip = (specParams.PageIndex - 1) * specParams.PageSize;
        Take = specParams.PageSize;
    }
}
