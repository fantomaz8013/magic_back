using Magic.DAL.Extensions;
using Magic.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Magic.DAL.Configurations;

public class DiceGameSessionMessageConfiguration : IEntityTypeConfiguration<DiceGameSessionMessage>
{
    public void Configure(EntityTypeBuilder<DiceGameSessionMessage> builder)
    {
        builder.PropertyWithUnderscore(x => x.Roll);
        builder.PropertyWithUnderscore(x => x.AuthorId);
        builder.HasForeignKey(x => x.Author, x => x.AuthorId);
        builder.HasOne(x => x.Author);

        builder.PropertyWithUnderscore(x => x.CubeTypeEnum)
            .HasConversion<int>();
    }
}