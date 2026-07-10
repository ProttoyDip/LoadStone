using Lodestone.Application.Interfaces;
using Lodestone.Domain.Entities;
using Lodestone.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Lodestone.Infrastructure.Repositories;

/// <summary>Journal-specific queries over MoodJournalEntries.</summary>
public class JournalRepository : GenericRepository<MoodJournalEntry>, IJournalRepository
{
    public JournalRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IReadOnlyList<MoodJournalEntry>> GetByStudentIdAsync(
        int studentProfileId, CancellationToken cancellationToken = default)
        => await Set
            .Where(e => e.StudentProfileId == studentProfileId && !e.IsDeleted)
            .OrderByDescending(e => e.EntryDateUtc)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

    public Task<bool> HasEntryForDayAsync(
        int studentProfileId,
        DateTime dayStartUtc,
        CancellationToken cancellationToken = default)
    {
        var nextDayStartUtc = dayStartUtc.AddDays(1);
        return Set.AnyAsync(entry =>
            entry.StudentProfileId == studentProfileId &&
            !entry.IsDeleted &&
            entry.EntryDateUtc >= dayStartUtc &&
            entry.EntryDateUtc < nextDayStartUtc,
            cancellationToken);
    }
}
