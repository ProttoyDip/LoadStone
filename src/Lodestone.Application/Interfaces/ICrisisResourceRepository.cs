using Lodestone.Domain.Entities;

namespace Lodestone.Application.Interfaces;

/// <summary>Read access to crisis resources. Implemented in Infrastructure.</summary>
public interface ICrisisResourceRepository
{
    Task<IReadOnlyList<CrisisResource>> GetActiveOrderedAsync(CancellationToken cancellationToken = default);
}
