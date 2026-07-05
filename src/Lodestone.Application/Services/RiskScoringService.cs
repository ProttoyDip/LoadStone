using Lodestone.Application.DTOs.Risk;
using Lodestone.Application.Interfaces;

namespace Lodestone.Application.Services;

/// <summary>
/// Coordinates feature retrieval + persistence around ML inference.
/// The actual model call is injected as an Application-owned interface so ML stays a plug-in.
/// </summary>
public class RiskScoringService : IRiskScoringService
{
    private readonly IUnitOfWork _unitOfWork;

    public RiskScoringService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public Task<RiskScoreDto> ScoreStudentAsync(int studentProfileId, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();

    public Task ScoreAllStudentsAsync(CancellationToken cancellationToken = default)
        => throw new NotImplementedException();

    public Task<IReadOnlyList<RiskQueueItemDto>> GetOpenQueueAsync(CancellationToken cancellationToken = default)
        => throw new NotImplementedException();
}
