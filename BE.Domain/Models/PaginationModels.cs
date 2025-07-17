using System.ComponentModel.DataAnnotations;

namespace BE.Domain.Models;

public class PagedResult<T>
{
    public IReadOnlyCollection<T> Items { get; init; } = Array.Empty<T>();
    public int TotalCount { get; init; }
    public int Page { get; init; }
    public int PageSize { get; init; }
    public int TotalPages => PageSize > 0 ? (int)Math.Ceiling((double)TotalCount / PageSize) : 0;
    public bool HasNextPage => Page < TotalPages;
    public bool HasPreviousPage => Page > 1;
    
    public int StartIndex => TotalCount > 0 ? (Page - 1) * PageSize + 1 : 0;
    public int EndIndex => Math.Min(Page * PageSize, TotalCount);
}

public class PaginationParameters
{
    private const int MaxPageSize = 100;
    private int _pageSize = 20;
    
    [Range(1, int.MaxValue, ErrorMessage = "Page must be greater than 0")]
    public int Page { get; set; } = 1;
    
    [Range(1, MaxPageSize, ErrorMessage = "PageSize must be between 1 and 100")]
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = Math.Clamp(value, 1, MaxPageSize);
    }
    
    [StringLength(100, ErrorMessage = "Search term cannot exceed 100 characters")]
    public string? Search { get; set; }
    
    public string? SortBy { get; set; }
    public SortDirection SortDirection { get; set; } = SortDirection.Ascending;
}

public enum SortDirection
{
    Ascending,
    Descending
}
