using Lodestone.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lodestone.Infrastructure.Data.Configurations;

public class CounselorBookingConfiguration : IEntityTypeConfiguration<CounselorBooking>
{
    public void Configure(EntityTypeBuilder<CounselorBooking> builder)
    {
        // TODO: configure keys, indexes, relationships and column constraints for CounselorBooking.
    }
}
