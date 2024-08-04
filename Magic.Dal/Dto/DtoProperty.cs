using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Magic.DAL.Dto;

public class DtoProperty<TEntity>
{
    public string Name { get; set; }
    public Expression<Func<TEntity, object>> Property { get; set; }
    public Expression<Func<TEntity, object>> Filter { get; set; }
    public Expression<Func<TEntity, object>> Sort { get; set; }
    public Expression<Func<TEntity, string>> Search { get; set; }

    public bool TryFilterExpression(string param, out Expression<Func<TEntity, bool>> predicate)
    {
        try
        {
            if (Filter == null)
                throw new ArgumentNullException("expression");

            if (param == null)
                throw new ArgumentNullException("param");

            if (!(Filter.Body is MemberExpression memberExpression))
                memberExpression = ((UnaryExpression)Filter.Body).Operand as MemberExpression;

            var converter = TypeDescriptor.GetConverter(memberExpression.Type);
            var propertyValue = converter.ConvertFromInvariantString(param);
            if (propertyValue.GetType() == typeof(DateTime))
                propertyValue = DateTime.SpecifyKind((DateTime)propertyValue, DateTimeKind.Utc);

            var expressionBody = Expression.Equal(memberExpression, Expression.Constant(propertyValue));

            predicate = Expression.Lambda<Func<TEntity, bool>>(expressionBody, Filter.Parameters);

            return true;
        }
        catch (Exception ex)
        {
            predicate = null;

            return false;
        }
    }

    public bool TrySortExpression(out Expression<Func<TEntity, object>> predicate)
    {
        try
        {
            if (Sort == null)
                throw new ArgumentNullException("expression");

            predicate = Sort;

            return true;
        }
        catch (Exception ex)
        {
            predicate = null;

            return false;
        }
    }

    public bool TryLikeExpression(string search, out Expression<Func<TEntity, bool>> predicate)
    {
        try
        {
            if (Search == null)
                throw new ArgumentNullException("expression");

            if (search == null)
                throw new ArgumentNullException("search");

            var concatMethod = typeof(string).GetMethod(nameof(string.Concat), new[] { typeof(string), typeof(string) });
            var searchLike = Expression.Lambda<Func<TEntity, bool>>(
                Expression.Call(
                    typeof(DbFunctionsExtensions),
                    nameof(DbFunctionsExtensions.Like),
                    null,
                    Expression.Constant(EF.Functions),
                    Search.Body,
                    Expression.Constant($"%{search}%")),
                Search.Parameters);

            predicate = searchLike;

            return true;
        }
        catch (Exception ex)
        {
            predicate = null;

            return false;
        }
    }
}