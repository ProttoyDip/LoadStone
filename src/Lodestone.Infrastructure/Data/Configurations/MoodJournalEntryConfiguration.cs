using Lodestone.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lodestone.Infrastructure.Data.Configurations;

public class MoodJournalEntryConfiguration : IEntityTypeConfiguration<MoodJournalEntry>
{
    public void Configure(EntityTypeBuilder<MoodJournalEntry> builder)
    {
        builder.Property<DateTime>("EntryDayUtc")
            .HasColumnType("date")
            .HasComputedColumnSql("CONVERT(date, [EntryDateUtc])", stored: true);

        builder.HasIndex(entry => new { entry.StudentProfileId, entry.EntryDateUtc });

        builder.HasIndex("StudentProfileId", "EntryDayUtc")
            .HasDatabaseName("UX_MoodJournalEntries_StudentProfileId_EntryDayUtc_Active")
            .HasFilter("[IsDeleted] = 0")
            .IsUnique();
    }
}
