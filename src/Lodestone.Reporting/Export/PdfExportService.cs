using Lodestone.Application.DTOs.Reports;
using Lodestone.Application.Interfaces;
using Lodestone.Reporting.Reports;

namespace Lodestone.Reporting.Export;

/// <summary>Implements the Application <see cref="IReportService"/> using QuestPDF generators.</summary>
public class PdfExportService : IReportService
{
    private readonly CounselorSessionReportGenerator _sessionGenerator;
    private readonly RiskSummaryReportGenerator _riskGenerator;
    private readonly StudentEngagementReportGenerator _engagementGenerator;

    public PdfExportService(
        CounselorSessionReportGenerator sessionGenerator,
        RiskSummaryReportGenerator riskGenerator,
        StudentEngagementReportGenerator engagementGenerator)
    {
        _sessionGenerator = sessionGenerator;
        _riskGenerator = riskGenerator;
        _engagementGenerator = engagementGenerator;
    }

    public Task<ReportResultDto> GenerateAsync(ReportRequestDto request, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();
}
