using Lodestone.Application.Interfaces;
using Lodestone.Reporting.Export;
using Lodestone.Reporting.Reports;
using Microsoft.Extensions.DependencyInjection;

namespace Lodestone.Reporting;

/// <summary>Registers QuestPDF report generators and the report service facade.</summary>
public static class DependencyInjection
{
    public static IServiceCollection AddReporting(this IServiceCollection services)
    {
        // QuestPDF community licence is set once in Program.cs.
        services.AddScoped<CounselorSessionReportGenerator>();
        services.AddScoped<RiskSummaryReportGenerator>();
        services.AddScoped<StudentEngagementReportGenerator>();
        services.AddScoped<IReportService, PdfExportService>();

        return services;
    }
}
