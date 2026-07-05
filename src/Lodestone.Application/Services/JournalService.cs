using Lodestone.Application.DTOs.Journal;
using Lodestone.Application.Interfaces;

namespace Lodestone.Application.Services;

public class JournalService : IJournalService
{
    private readonly IUnitOfWork _unitOfWork;

    public JournalService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public Task<IReadOnlyList<JournalEntryDto>> GetEntriesAsync(int studentProfileId, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();

    public Task<JournalEntryDto> AddEntryAsync(int studentProfileId, CreateJournalEntryDto dto, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();
}
