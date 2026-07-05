using Lodestone.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lodestone.Infrastructure.Data.Configurations;

public class CounselorSessionReportConfiguration : IEntityTypeConfiguration<CounselorSessionReport>
{
    public void Configure(EntityTypeBuilder<CounselorSessionReport> builder)
    {
        // TODO: configure keys, indexes, relationships and column constraints for CounselorSessionReport.
    }
}
