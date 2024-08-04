using System.Linq.Expressions;

namespace Magic.DAL.Dto;

public class DtoBuilder<TEntity>
{
    private Dictionary<string, DtoProperty<TEntity>> _propertis = new();
    public DtoProperty<TEntity> AddProperty(Expression<Func<TEntity, object>> propertyExpression)
    {
        if (propertyExpression == null)
            throw new ArgumentNullException("propertyExpression");

        if (!(propertyExpression.Body is MemberExpression memberExpression))
            memberExpression = ((UnaryExpression)propertyExpression.Body).Operand as MemberExpression;

        return AddProperty(propertyExpression, memberExpression.Member.Name);
    }
    public DtoProperty<TEntity> AddProperty(Expression<Func<TEntity, object>> propertyExpression, string propertyName)
    {
        if (propertyExpression == null)
            throw new ArgumentNullException("propertyExpression");
        if (string.IsNullOrEmpty(propertyName))
            throw new ArgumentNullException("propertyName");

        var property = new DtoProperty<TEntity>();
        property.Name = propertyName;
        property.Property = propertyExpression;
        property.Filter = propertyExpression;
        property.Sort = propertyExpression;

        _propertis.Add(propertyName.ToLowerInvariant(), property);

        return property;
    }
    public bool TryGetProperty(string propertyName, out DtoProperty<TEntity> property)
    {
        if (string.IsNullOrEmpty(propertyName))
        {
            property = null;
            return false;
        }
        return _propertis.TryGetValue(propertyName.ToLowerInvariant(), out property);
    }
}