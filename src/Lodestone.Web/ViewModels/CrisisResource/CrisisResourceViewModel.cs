using CrisisResourceEntity = Lodestone.Domain.Entities.CrisisResource;

namespace Lodestone.Web.ViewModels.CrisisResource;

public class CrisisResourceViewModel
{
    public IReadOnlyList<CrisisResourceEntity> EmergencyResources { get; init; } = Array.Empty<CrisisResourceEntity>();
    public IReadOnlyList<CrisisResourceEntity> SupportResources { get; init; } = Array.Empty<CrisisResourceEntity>();
}
