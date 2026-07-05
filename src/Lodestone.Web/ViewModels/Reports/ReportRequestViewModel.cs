using System.ComponentModel.DataAnnotations;

namespace Lodestone.Web.ViewModels.Reports;

public class ReportRequestViewModel
{
    [Required] public string ReportType { get; set; } = "RiskSummary";
    [DataType(DataType.Date)] public DateTime FromUtc { get; set; }
    [DataType(DataType.Date)] public DateTime ToUtc { get; set; }
    public int? TargetId { get; set; }
}
