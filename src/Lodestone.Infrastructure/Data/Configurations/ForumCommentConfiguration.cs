using Lodestone.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lodestone.Infrastructure.Data.Configurations;

public class ForumCommentConfiguration : IEntityTypeConfiguration<ForumComment>
{
    public void Configure(EntityTypeBuilder<ForumComment> builder)
    {
        // TODO: configure keys, indexes, relationships and column constraints for ForumComment.
    }
}
