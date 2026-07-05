using Lodestone.Application.DTOs.Reports;

namespace Lodestone.Application.Interfaces;

/// <summary>Report facade for the Web layer; concrete PDF generation lives in Lodestone.Reporting.</summary>
public interface IReportService
{
    Task<ReportResultDto> GenerateAsync(ReportRequestDto request, CancellationToken cancellationToken = default);
}
