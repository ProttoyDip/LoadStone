using Lodestone.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lodestone.Infrastructure.Data.Configurations;

public class NudgeConfiguration : IEntityTypeConfiguration<Nudge>
{
    public void Configure(EntityTypeBuilder<Nudge> builder)
    {
        // TODO: configure keys, indexes, relationships and column constraints for Nudge.
    }
}
