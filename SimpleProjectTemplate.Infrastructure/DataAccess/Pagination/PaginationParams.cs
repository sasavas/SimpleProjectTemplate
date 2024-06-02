namespace SimpleProjectTemplate.Infrastructure.DataAccess.Pagination;

public record PaginationParams
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
