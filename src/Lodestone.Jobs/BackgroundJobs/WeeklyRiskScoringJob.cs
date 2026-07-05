using Lodestone.Application.Interfaces;

namespace Lodestone.Jobs.BackgroundJobs;

/// <summary>Recurring Hangfire job: recompute risk scores for all students weekly.</summary>
public class WeeklyRiskScoringJob
{
    private readonly IRiskScoringService _riskScoringService;

    public WeeklyRiskScoringJob(IRiskScoringService riskScoringService)
        => _riskScoringService = riskScoringService;

    public Task ExecuteAsync(CancellationToken cancellationToken = default)
        => _riskScoringService.ScoreAllStudentsAsync(cancellationToken);
}
