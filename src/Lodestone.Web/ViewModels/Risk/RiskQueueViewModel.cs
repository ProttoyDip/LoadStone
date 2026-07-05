using Lodestone.Application.DTOs.Risk;

namespace Lodestone.Web.ViewModels.Risk;

public class RiskQueueViewModel
{
    public IReadOnlyList<RiskQueueItemDto> Items { get; set; } = new List<RiskQueueItemDto>();
}
