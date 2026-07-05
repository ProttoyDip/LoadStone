namespace Lodestone.Application.Interfaces;

public interface ICrisisResourceService
{
    Task<IReadOnlyList<Domain.Entities.CrisisResource>> GetActiveResourcesAsync(CancellationToken cancellationToken = default);
}
