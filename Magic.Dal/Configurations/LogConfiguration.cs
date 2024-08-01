using Magic.DAL.Extensions;
using Magic.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Magic.DAL.Configurations
{
    public class LogConfiguration : IEntityTypeConfiguration<Log>
    {
        public void Configure(EntityTypeBuilder<Log> builder)
        {
            builder.HasTableNameUnderscoreStyle(nameof(Log));
            builder.HasBaseEntityInt();
            builder.HasCreatedDateEntity();

            builder.PropertyWithUnderscore(x => x.Level).HasConversion<int>();
            builder.PropertyWithUnderscore(x => x.Category).HasConversion<int>();
            builder.PropertyWithUnderscore(x => x.Text);
        }
    }
}
