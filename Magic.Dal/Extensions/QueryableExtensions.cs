using Magic.DAL.Dto;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace Magic.DAL.Extensions
{
    public record class PagedRequest(int Page = 0, int Size = 0, IEnumerable<string>? OrderBy = null, IEnumerable<string>? Filter = null, IEnumerable<string>? Searches = null);
    public static class QueryableExtensions
    {
        private const string SEPARATOR = ":";

        public static IQueryable<TEntity> ApplyFilter<TEntity, TReturn>(this IQueryable<TEntity> query, DtoBuilder<TEntity> builder, IEnumerable<string>? filters)
            where TEntity : class
            where TReturn : IDtoConfiguration<TEntity>, new()
        {
            if (filters == null)
                return query;

            IQueryable<TEntity> p = query;
            foreach (var param in filters.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()))
            {
                var separatorIndex = param.IndexOf(SEPARATOR);
                var expressionLeft = param.Substring(0, separatorIndex);
                var expressionRight = param.Substring(separatorIndex + 1);

                DtoProperty<TEntity> dtoProperty;
                if (!builder.TryGetProperty(expressionLeft, out dtoProperty))
                    continue;

                Expression<Func<TEntity, bool>> predicate;
                if (!dtoProperty.TryFilterExpression(expressionRight, out predicate))
                    continue;

                p = p.Where(predicate);
            }

            return p ?? query;
        }
        public static IQueryable<TEntity> ApplySearch<TEntity, TReturn>(this IQueryable<TEntity> query, DtoBuilder<TEntity> builder, IEnumerable<string>? searches)
            where TEntity : class
            where TReturn : IDtoConfiguration<TEntity>, new()
        {
            if (searches == null)
                return query;

            IQueryable<TEntity> p = query;
            foreach (var param in searches.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()))
            {
                var separatorIndex = param.IndexOf(SEPARATOR);
                var expressionLeft = param.Substring(0, separatorIndex);
                var expressionRight = param.Substring(separatorIndex + 1);

                DtoProperty<TEntity> dtoProperty;
                if (!builder.TryGetProperty(expressionLeft, out dtoProperty))
                    continue;

                Expression<Func<TEntity, bool>> predicate;
                if (!dtoProperty.TryLikeExpression(expressionRight, out predicate))
                    continue;

                p = p.Where(predicate);
            }

            return p ?? query;
        }
        public static IQueryable<TEntity>? ApplySort<TEntity, TReturn>(this IQueryable<TEntity> query, DtoBuilder<TEntity> builder, IEnumerable<string>? orders)
           where TEntity : class
           where TReturn : IDtoConfiguration<TEntity>, new()
        {
            if (orders == null)
                return query;

            IOrderedQueryable<TEntity> p = null;
            foreach (var param in orders.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()))
            {
                var propertyName = param.Split(SEPARATOR)[0];

                DtoProperty<TEntity> dtoProperty;
                if (!builder.TryGetProperty(propertyName, out dtoProperty))
                    continue;

                Expression<Func<TEntity, dynamic>> predicate;
                if (!dtoProperty.TrySortExpression(out predicate))
                    continue;

                if (param.EndsWith(":desc"))
                    if (p == null)
                        p = query.OrderByDescending(predicate);
                    else
                        p = p.ThenByDescending(predicate);
                else
                    if (p == null)
                    p = query.OrderBy(predicate);
                else
                    p = p.ThenBy(predicate);
            }

            return p ?? query;
        }
        public static async Task<PagedResult<TReturn>> GetPagedAsync<TEntity, TReturn>(this IQueryable<TEntity> query, int pageIndex, int pageSize, Expression<Func<TEntity, TReturn>> selector)
           where TEntity : class
           where TReturn : IDtoConfiguration<TEntity>, new()
        {
            var result = new PagedResult<TReturn>();
            result.CurrentPage = pageIndex;
            result.PageSize = pageSize;
            result.RowCount = await query.CountAsync();

            var pageCount = pageSize == 0 ? 0 : result.RowCount / pageSize;
            result.PageCount = pageCount;

            var skip = (pageIndex - 1) * pageSize;

            result.Queryable = query
                .Select(selector)
                .Skip(skip)
                .Take(pageSize);

            return result;
        }
        public static async Task<PagedResult<TReturn>> ApplyRequestAsync<TEntity, TReturn>(this IQueryable<TEntity> query, PagedRequest request, Expression<Func<TEntity, TReturn>> selector)
           where TEntity : class
           where TReturn : IDtoConfiguration<TEntity>, new()
        {
            var builder = new DtoBuilder<TEntity>();
            var dto = new TReturn();
            dto.Configure(builder);

            return await query
                .ApplyFilter<TEntity, TReturn>(builder, request.Filter)
                .ApplySearch<TEntity, TReturn>(builder, request.Searches)
                .ApplySort<TEntity, TReturn>(builder, request.OrderBy)
                .GetPagedAsync(request.Page, request.Size, selector);
        }
    }
}
