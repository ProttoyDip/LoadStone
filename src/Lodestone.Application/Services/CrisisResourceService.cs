using Lodestone.Application.Interfaces;
using Lodestone.Domain.Entities;

namespace Lodestone.Application.Services;

public class CrisisResourceService : ICrisisResourceService
{
    private readonly ICrisisResourceRepository _repository;

    public CrisisResourceService(ICrisisResourceRepository repository) => _repository = repository;

    public Task<IReadOnlyList<CrisisResource>> GetActiveResourcesAsync(CancellationToken cancellationToken = default)
        => _repository.GetActiveOrderedAsync(cancellationToken);
}
