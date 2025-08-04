using Microsoft.EntityFrameworkCore;

namespace Api.DTOs
{
    public class PageResponse<TResult>
    {
        public IEnumerable<TResult> Data { get; set; } = [];
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
    }

    public static class PageResponseExtensions
    {
        public static IQueryable<T> ToPage<T>(this IQueryable<T> queryable, int currentPage, int pageSize)
            => queryable.Skip(pageSize * (currentPage - 1))
                .Take(pageSize);

        public static async Task<PageResponse<TDto>> ToPageResponseAsync<TEntity, TDto>(this IQueryable<TEntity> queryable, int currentPage, int pageSize, Func<TEntity, TDto> mapper, CancellationToken ct = default)
        {
            var queryablePaged = queryable.Skip(pageSize * (currentPage - 1))
                .Take(pageSize);

            var totalItems = await queryablePaged.CountAsync(ct);
            var items = await queryablePaged.ToListAsync(ct);

            return new PageResponse<TDto>()
            {
                Data = [.. items.Select(x => mapper(x))],
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
            };
        }

        public static async Task<PageResponse<T>> ToPageResponseAsync<T>(this IQueryable<T> queryable, int currentPage, int pageSize, CancellationToken ct = default)
        {
            var queryablePaged = queryable.Skip(pageSize * (currentPage - 1))
                .Take(pageSize);

            var totalItems = await queryablePaged.CountAsync(ct);

            return new PageResponse<T>()
            {
                Data = queryablePaged,
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
            };
        }
    }
}
