using Lodestone.Application.Interfaces;

namespace Lodestone.Jobs.BackgroundJobs;

/// <summary>Sweeps flagged forum content for automated moderation triage.</summary>
public class ForumModerationJob
{
    private readonly IForumService _forumService;

    public ForumModerationJob(IForumService forumService) => _forumService = forumService;

    public Task ExecuteAsync(CancellationToken cancellationToken = default)
        => throw new NotImplementedException();
}
