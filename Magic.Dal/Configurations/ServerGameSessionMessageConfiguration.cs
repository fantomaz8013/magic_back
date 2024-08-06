using Magic.DAL.Extensions;
using Magic.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Magic.DAL.Configurations;

public class ServerGameSessionMessageConfiguration : IEntityTypeConfiguration<ServerGameSessionMessage>
{
    public void Configure(EntityTypeBuilder<ServerGameSessionMessage> builder)
    {
        builder.PropertyWithUnderscore(x => x.Message);
    }
}