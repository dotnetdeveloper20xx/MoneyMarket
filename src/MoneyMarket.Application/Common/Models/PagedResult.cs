namespace MoneyMarket.Application.Common.Models
{
    public sealed class PagedResult<T>
    {
        public IReadOnlyList<T> Items { get; init; }
        public int PageNumber { get; init; }
        public int PageSize { get; init; }
        public int TotalCount { get; init; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public bool HasNextPage => PageNumber < TotalPages;
        public bool HasPreviousPage => PageNumber > 1;


        private PagedResult(IReadOnlyList<T> items, int pageNumber, int pageSize, int totalCount)
        => (Items, PageNumber, PageSize, TotalCount) = (items, pageNumber, pageSize, totalCount);


        public static PagedResult<T> Create(IReadOnlyList<T> items, int pageNumber, int pageSize, int totalCount)
        => new(items, pageNumber, pageSize, totalCount);
    }
}
