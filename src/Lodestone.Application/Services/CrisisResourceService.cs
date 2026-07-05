using Lodestone.Application.Interfaces;
using Lodestone.Domain.Entities;

namespace Lodestone.Application.Services;

public class CrisisResourceService : ICrisisResourceService
{
    private readonly IUnitOfWork _unitOfWork;

    public CrisisResourceService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public Task<IReadOnlyList<CrisisResource>> GetActiveResourcesAsync(CancellationToken cancellationToken = default)
        => throw new NotImplementedException();
}
