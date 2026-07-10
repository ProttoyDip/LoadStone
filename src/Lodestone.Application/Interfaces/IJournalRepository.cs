using Lodestone.Domain.Entities;

namespace Lodestone.Application.Interfaces;

/// <summary>Journal-specific queries. Implemented in Infrastructure.</summary>
public interface IJournalRepository
{
    Task<IReadOnlyList<MoodJournalEntry>> GetByStudentIdAsync(int studentProfileId, CancellationToken cancellationToken = default);
    Task<bool> HasEntryForDayAsync(int studentProfileId, DateTime dayStartUtc, CancellationToken cancellationToken = default);
    Task AddAsync(MoodJournalEntry entry, CancellationToken cancellationToken = default);
}
