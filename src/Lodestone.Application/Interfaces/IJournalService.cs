using Lodestone.Application.DTOs.Journal;

namespace Lodestone.Application.Interfaces;

public interface IJournalService
{
    Task<IReadOnlyList<JournalEntryDto>> GetEntriesAsync(int studentProfileId, CancellationToken cancellationToken = default);
    Task<JournalEntryDto> AddEntryAsync(int studentProfileId, CreateJournalEntryDto dto, CancellationToken cancellationToken = default);
}
