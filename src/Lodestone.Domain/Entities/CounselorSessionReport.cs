using Lodestone.Domain.Common;
using Lodestone.Domain.Enums;

namespace Lodestone.Domain.Entities;

/// <summary>Post-session write-up produced by a counselor; source data for QuestPDF reports.</summary>
public class CounselorSessionReport : AuditableEntity
{
    public int CounselorBookingId { get; set; }
    public CounselorBooking? Booking { get; set; }

    public string Summary { get; set; } = string.Empty;
    public string? Recommendations { get; set; }
    public ReportStatus Status { get; set; } = ReportStatus.Draft;
}
