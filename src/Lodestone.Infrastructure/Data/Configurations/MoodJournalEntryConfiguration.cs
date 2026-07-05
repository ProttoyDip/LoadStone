using Lodestone.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lodestone.Infrastructure.Data.Configurations;

public class MoodJournalEntryConfiguration : IEntityTypeConfiguration<MoodJournalEntry>
{
    public void Configure(EntityTypeBuilder<MoodJournalEntry> builder)
    {
        // TODO: configure keys, indexes, relationships and column constraints for MoodJournalEntry.
    }
}
