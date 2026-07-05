using Lodestone.Application.DTOs.Risk;

namespace Lodestone.Web.ViewModels.Counselor;

public class CounselorQueueViewModel
{
    public IReadOnlyList<RiskQueueItemDto> Queue { get; set; } = new List<RiskQueueItemDto>();
}
