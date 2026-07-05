using Lodestone.Domain.Entities;

namespace Lodestone.Reporting.Reports;

/// <summary>Produces a PDF for a single counselor session. Generators live only in Lodestone.Reporting.</summary>
public class CounselorSessionReportGenerator
{
    public byte[] Generate(CounselorSessionReport report)
        => throw new NotImplementedException();
}
