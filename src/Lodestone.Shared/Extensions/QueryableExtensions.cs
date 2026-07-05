namespace Lodestone.Shared.Extensions;

public static class QueryableExtensions
{
    /// <summary>Applies simple skip/take paging.</summary>
    public static IQueryable<T> Page<T>(this IQueryable<T> source, int pageNumber, int pageSize)
        => source.Skip((Math.Max(pageNumber, 1) - 1) * pageSize).Take(pageSize);
}
