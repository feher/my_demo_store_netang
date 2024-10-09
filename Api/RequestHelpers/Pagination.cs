namespace Api.RequestHelpers;

public class Pagination<T>
{
    public int PageIndex { get; init; }
    public int PageSize { get; init; }
    public int Count { get; init; }
    public required IReadOnlyList<T> Data { get; init; }
}
