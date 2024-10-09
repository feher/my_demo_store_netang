namespace Core.Specifications;

public class ProductSpecParams
{
    private List<string> _brands = [];

    /// <summary>
    /// The setter splits up comma separated strings.
    /// So, ["brand1,brand2,brand3", "brand4"] becomes
    /// ["brand1", "brand2", "brand3", "brand4"]
    /// </summary>
    public List<string> Brands
    {
        get => _brands;
        set => _brands = value.SelectMany(s => s.Split(',', StringSplitOptions.RemoveEmptyEntries)).ToList();
    }

    private List<string> _types = [];

    /// <summary>
    /// The setter splits up comma separated strings.
    /// So, ["brand1,brand2,brand3", "brand4"] becomes
    /// ["brand1", "brand2", "brand3", "brand4"]
    /// </summary>
    public List<string> Types
    {
        get => _types;
        set => _types = value.SelectMany(s => s.Split(',', StringSplitOptions.RemoveEmptyEntries)).ToList();
    }

    private string? _searchTerm;
    public string? SearchTerm
    {
        get => _searchTerm;
        set => _searchTerm = string.IsNullOrWhiteSpace(value) ? null : value.ToLower();
    }

    public string SortBy { get; set; } = "";

    public bool IsPagingEnabled { get; set; } = false;

    /// <summary>
    /// 1-based index.
    /// </summary>
    public int PageIndex { get; set; }

    private const int MaxPageSize = 50;
    private int _pageSize;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = Math.Min(MaxPageSize, value);
    }

    public string SelectField { get; set; } = "";

    public bool IsSelectionDistinct { get; set; } = false;
}
