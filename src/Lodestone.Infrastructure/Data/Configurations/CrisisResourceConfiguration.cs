using Lodestone.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lodestone.Infrastructure.Data.Configurations;

public class CrisisResourceConfiguration : IEntityTypeConfiguration<CrisisResource>
{
    public void Configure(EntityTypeBuilder<CrisisResource> builder)
    {
        // TODO: configure keys, indexes, relationships and column constraints for CrisisResource.
    }
}
