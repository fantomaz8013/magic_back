using System;
using System.Linq.Expressions;

namespace Magic.DAL.Dto
{
    public static class DtoBuilderExtension
    {
        public static DtoProperty<TEntity> AddSort<TEntity>(this DtoProperty<TEntity> property, Expression<Func<TEntity, object>> sortExpression)
        {
            if (sortExpression == null)
                throw new ArgumentNullException("sortExpression");

            property.Sort = sortExpression;

            return property;
        }
        public static DtoProperty<TEntity> AddFilter<TEntity>(this DtoProperty<TEntity> property, Expression<Func<TEntity, object>> filterExpression)
        {
            if (filterExpression == null)
                throw new ArgumentNullException("filterExpression");

            property.Filter = filterExpression;

            return property;
        }
        public static DtoProperty<TEntity> AddSearch<TEntity>(this DtoProperty<TEntity> property, Expression<Func<TEntity, string>> searchExpression)
        {
            if (searchExpression == null)
                throw new ArgumentNullException("searchExpression");

            property.Search = searchExpression;

            return property;
        }
    }
}
