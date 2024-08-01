using Magic.DAL.Extensions;
using Magic.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Magic.DAL.Configurations
{
    public class CityConfiguration : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            builder.HasTableNameUnderscoreStyle(nameof(City));
            builder.HasBaseEntityInt();

            builder.PropertyWithUnderscore(x => x.Title);

            builder.HasData(new City { Id = 1, Title = "Казань" });
            builder.HasData(new City { Id = 2, Title = "Москва" });
            builder.HasData(new City { Id = 3, Title = "Екатеринбург" });
        }
    }
}
