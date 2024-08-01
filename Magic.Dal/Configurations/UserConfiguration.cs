using Magic.DAL.Extensions;
using Magic.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Magic.DAL.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasTableNameUnderscoreStyle(nameof(User));
            builder.HasBaseEntityGuid();
            builder.HasCreatedDateEntity();
            builder.HasBlockedEntity();

            builder.PropertyWithUnderscore(x => x.Login);
            builder.PropertyWithUnderscore(x => x.Name);
            builder.PropertyWithUnderscore(x => x.Email);
            builder.PropertyWithUnderscore(x => x.Description);
            builder.PropertyWithUnderscore(x => x.AvatarUrl);
            builder.PropertyWithUnderscore(x => x.GameExperience);
            builder.PropertyWithUnderscore(x => x.PhoneNumber);
            builder.PropertyWithUnderscore(x => x.PasswordHash);
            builder.PropertyWithUnderscore(x => x.PasswordSalt);
            builder.PropertyWithUnderscore(x => x.RefKey);
            builder.PropertyWithUnderscore(x => x.RefUserId);
            builder.PropertyWithUnderscore(x => x.CityId);
            builder.HasForeignKey(x => x.City, x => x.CityId);
            builder.HasOne(x => x.City);
            /*builder.PropertyWithUnderscore(x => x.Code);
            builder.PropertyWithUnderscore(x => x.CodeDate)
                .HasDateTimeConversion()
                .HasColumnType(SqlColumnTypes.TimeStampWithTimeZone); */

            builder.HasForeignKey(x => x.RefUser, x => x.RefUserId);
        }
    }
}
