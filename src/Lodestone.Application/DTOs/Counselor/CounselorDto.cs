namespace Lodestone.Application.DTOs.Counselor;

public record CounselorDto(int Id, string UserId, string FullName, string? Specialization, bool IsAcceptingBookings);

public record AvailabilitySlotDto(int Id, int CounselorProfileId, DateTime StartUtc, DateTime EndUtc, bool IsBooked);
