using Magic.DAL.Extensions;
using Magic.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Magic.DAL.Configurations
{
    public class CharacterAvatarConfiguration : IEntityTypeConfiguration<CharacterAvatar>
    {
        public void Configure(EntityTypeBuilder<CharacterAvatar> builder)
        {
            builder.HasTableNameUnderscoreStyle(nameof(CharacterAvatar));
            builder.HasBaseEntityInt();

            builder.PropertyWithUnderscore(x => x.AvatarUrl);

            builder.HasData(new CharacterAvatar { Id = 1, AvatarUrl = "storage/character/avatar/1.png" });
            builder.HasData(new CharacterAvatar { Id = 2, AvatarUrl = "storage/character/avatar/2.png" });
            builder.HasData(new CharacterAvatar { Id = 3, AvatarUrl = "storage/character/avatar/3.png" });
            builder.HasData(new CharacterAvatar { Id = 4, AvatarUrl = "storage/character/avatar/4.png" });
            builder.HasData(new CharacterAvatar { Id = 5, AvatarUrl = "storage/character/avatar/5.png" });
        }
    }
}
