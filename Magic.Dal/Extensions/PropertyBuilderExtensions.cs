using Magic.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Linq.Expressions;

namespace Magic.DAL.Extensions;

public static class PropertyBuilderExtensions
{
    public static void HasCreatedDateEntity<T>(this EntityTypeBuilder<T> builder)
        where T : class, IHasCreatedDate
        => builder.PropertyWithUnderscore(x => x.CreatedDate)
            .HasDateTimeConversion()
            .HasColumnType(SqlColumnTypes.TimeStampWithTimeZone);

    public static void HasBlockedEntity<T>(this EntityTypeBuilder<T> builder)
        where T : class, IHasBlocked
    {
        builder.PropertyWithUnderscore(x => x.IsBlocked);
        builder.PropertyWithUnderscore(x => x.BlockedDate)
            .HasDateTimeConversion()
            .HasColumnType(SqlColumnTypes.TimeStampWithTimeZone)
            .IsRequired(false);
    }

    public static PropertyBuilder<DateTime> HasDateTimeConversion(this PropertyBuilder<DateTime> builder)
        => builder.HasConversion(
            v => v,
            v => v.ToUniversalTime());

    public static PropertyBuilder<DateTime?> HasDateTimeConversion(this PropertyBuilder<DateTime?> builder)
        => builder.HasConversion(
            v => v,
            v => v.HasValue ? v.Value.ToUniversalTime() : null);


    #region Naming Column and Table

    public static void HasBaseEntityInt<T>(this EntityTypeBuilder<T> builder)
        where T : BaseEntity<int>
    {
        builder.HasKey(x => x.Id);
        builder.PropertyWithUnderscore(x => x.Id).HasColumnNameUnderscoreStyle(nameof(BaseEntity<int>.Id));
    }

    public static void HasBaseEntityLong<T>(this EntityTypeBuilder<T> builder)
        where T : BaseEntity<long>
    {
        builder.HasKey(x => x.Id);
        builder.PropertyWithUnderscore(x => x.Id).HasColumnNameUnderscoreStyle(nameof(BaseEntity<long>.Id));
    }

    public static void HasBaseEntityGuid<T>(this EntityTypeBuilder<T> builder)
        where T : BaseEntity<Guid>
    {
        builder.HasKey(x => x.Id);
        builder.PropertyWithUnderscore(x => x.Id).HasColumnNameUnderscoreStyle(nameof(BaseEntity<Guid>.Id));
    }

    public static EntityTypeBuilder<T> HasTableNameUnderscoreStyle<T>(this EntityTypeBuilder<T> builder, string fieldName) where T : class
        => builder.ToTable(fieldName.ToLowerCaseWithUnderscore());

    public static EntityTypeBuilder<T> HasViewNameUnderscoreStyle<T>(this EntityTypeBuilder<T> builder, string fieldName) where T : class
        => builder.ToView(fieldName.ToLowerCaseWithUnderscore());

    public static PropertyBuilder<T> HasColumnNameUnderscoreStyle<T>(this PropertyBuilder<T> builder, string fieldName)
        => builder.HasColumnName(fieldName.ToLowerCaseWithUnderscore());

    public static PropertyBuilder<TProperty> PropertyWithUnderscore<TEntity, TProperty>(
        this EntityTypeBuilder<TEntity> builder,
        Expression<Func<TEntity, TProperty>> propertyExpression) where TEntity : class =>
        builder
            .Property(propertyExpression)
            .HasColumnNameUnderscoreStyle(propertyExpression.GetMemberAccess().Name.ToLowerCaseWithUnderscore());

    public static ReferenceCollectionBuilder<TRelatedEntity, TEntity> HasForeignKey<TEntity, TRelatedEntity>(
        this EntityTypeBuilder<TEntity> builder, 
        Expression<Func<TEntity, TRelatedEntity>> propertyExpression,
        Expression<Func<TEntity, object?>> foreignKeyExpression,
        DeleteBehavior deleteBehavior = DeleteBehavior.Restrict) 
        where TEntity : class 
        where TRelatedEntity : class =>
        builder
            .HasOne(propertyExpression)
            .WithMany()
            .HasForeignKey(foreignKeyExpression)
            .OnDelete(deleteBehavior);

    #endregion
}