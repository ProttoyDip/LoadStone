using Lodestone.Domain.Entities;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace Lodestone.Reporting.Templates;

/// <summary>QuestPDF document describing the layout of a counselor session report.</summary>
public class CounselorSessionTemplate : IDocument
{
    private readonly CounselorSessionReport _report;

    public CounselorSessionTemplate(CounselorSessionReport report) => _report = report;

    public void Compose(IDocumentContainer container)
        => throw new NotImplementedException();
}
