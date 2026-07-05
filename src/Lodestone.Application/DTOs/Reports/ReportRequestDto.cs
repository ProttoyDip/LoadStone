namespace Lodestone.Application.DTOs.Reports;

public record ReportRequestDto(string ReportType, DateTime FromUtc, DateTime ToUtc, int? TargetId);

public record ReportResultDto(string FileName, string ContentType, byte[] Content);
