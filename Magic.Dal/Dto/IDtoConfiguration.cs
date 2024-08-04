namespace Magic.DAL.Dto;

public interface IDtoConfiguration<TEntity>
    where TEntity : class
{
    public void Configure(DtoBuilder<TEntity> builder);
}