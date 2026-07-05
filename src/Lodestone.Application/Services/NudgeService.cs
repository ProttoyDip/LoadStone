using Lodestone.Application.Interfaces;

namespace Lodestone.Application.Services;

public class NudgeService : INudgeService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailService _emailService;

    public NudgeService(IUnitOfWork unitOfWork, IEmailService emailService)
    {
        _unitOfWork = unitOfWork;
        _emailService = emailService;
    }

    public Task GenerateNudgesForAtRiskStudentsAsync(CancellationToken cancellationToken = default)
        => throw new NotImplementedException();

    public Task DispatchPendingNudgesAsync(CancellationToken cancellationToken = default)
        => throw new NotImplementedException();
}
