using Magic.DAL.Extensions;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace Magic.Service
{
    public interface IBaseEntityService<TEntity, TReturn>
        where TEntity : class
        where TReturn : class
    {
        Task<TReturn?> GetByIdAsync(Guid id);
        Task<PagedResult<TReturn>> ListAsync(PagedRequest request);
        Task<IEnumerable<TReturn>> ListAsync(Expression<Func<TEntity, bool>> predicate);
    }
}
