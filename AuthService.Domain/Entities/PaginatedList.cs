using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Domain.Entities;

public class PaginatedList<T> : List<T>
{
    public int? PageIndex { get; private set; }

    public int? PageSize { get; private set; }

    public int TotalCount { get; private set; }

    public int TotalPages { get; private set; }

    public bool HasPreviousPage => PageIndex > 1;

    public bool HasNextPage => PageIndex < TotalPages;

    public string SortColumn { get; set; }

    public string SortOrder { get; set; }

    private PaginatedList(
        List<T> items,
        int count,
        int? pageIndex = null,
        int? pageSize = null,
        string? sortColumn = null,
        string? sortOrder = null)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
        TotalCount = count;
        TotalPages = pageSize == null ? 1 : (int)Math.Ceiling(count / (double)pageSize);
        SortColumn = sortColumn;
        SortOrder = sortOrder;
        this.AddRange(items);
    }

    public static async Task<PaginatedList<T>> CreateAsync(
        IQueryable<T> source,
        int? pageIndex = null,
        int? pageSize = null,
        string? sortColumn = null,
        string? sortOrder = null)
    {
        var count = await source.CountAsync();

        if (!string.IsNullOrEmpty(sortColumn)
            && IsValidProperty(sortColumn))
        {
            source = !string.IsNullOrEmpty(sortOrder) && sortOrder.ToUpper() == "ASC"
                ? source.OrderBy(x => GetProperty(sortColumn))
                : source.OrderByDescending(x => GetProperty(sortColumn));
        }

        if (pageSize != null && pageIndex != null)
            source = source.Skip(pageIndex.GetValueOrDefault(0) * pageSize.GetValueOrDefault(0));
        if (pageSize != null) source = source.Take(pageSize.GetValueOrDefault(0));

        var data = await source.ToListAsync();

        return new PaginatedList<T>(
            data,
            count,
            pageIndex,
            pageSize,
            sortColumn,
            sortOrder);
    }

    private static bool IsValidProperty(
        string propertyName,
        bool throwExceptionIfNotFound = true)
    {
        var prop = GetProperty(propertyName);

        if (prop == null && throwExceptionIfNotFound)
            throw new NotSupportedException(
                string.Format(
                    "ERROR: Property '{0}' does not exist.",
                    propertyName)
            );

        return prop != null;
    }

    private static PropertyInfo? GetProperty(string propertyName)
    {
        return typeof(T).GetProperty(
            propertyName,
            BindingFlags.IgnoreCase |
            BindingFlags.Public |
            BindingFlags.Static |
            BindingFlags.Instance);
    }
}