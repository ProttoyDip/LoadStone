using Lodestone.Domain.Enums;

namespace Lodestone.Application.DTOs.Booking;

public record BookingDto(int Id, int CounselorProfileId, int StudentProfileId, DateTime ScheduledForUtc, BookingStatus Status);

public record CreateBookingDto(int CounselorProfileId, int? AvailabilitySlotId, DateTime ScheduledForUtc, string? Notes);
