using Magic.DAL.Extensions;
using Magic.Domain.Entities;
using Magic.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Magic.DAL.Configurations;

public class ChatGameSessionMessageConfiguration : IEntityTypeConfiguration<ChatGameGameSessionMessage>
{
    public void Configure(EntityTypeBuilder<ChatGameGameSessionMessage> builder)
    {
        builder.PropertyWithUnderscore(x => x.AuthorId);
        builder.HasForeignKey(x => x.Author, x => x.AuthorId);
        builder.HasOne(x => x.Author);
    }
}