using Core.Specifications;

namespace Api.RequestHelpers;

public class ProductParams
{
    public string Search { get; set; } = "";

    public List<string> Brands { get; set; } = [];

    public List<string> Types { get; set; } = [];

    public string Sort { get; set; } = "";

    /// <summary>
    /// 1-based index.
    /// </summary>
    public int PageIndex { get; set; }

    public int PageSize {get; set; }
}

public static class ProductParamsExtensions
{
    public static ProductSpecParams ToSpecParams(this ProductParams paramz)
    {
        return new ProductSpecParams()
        {
            SearchTerm = paramz.Search,
            Brands = paramz.Brands,
            Types = paramz.Types,
            SortBy = paramz.Sort,
            IsPagingEnabled = paramz.PageIndex != 0,
            PageIndex = paramz.PageIndex,
            PageSize = paramz.PageSize
        };
    }
}
