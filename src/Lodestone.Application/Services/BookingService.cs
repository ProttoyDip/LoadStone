using Lodestone.Application.DTOs.Booking;
using Lodestone.Application.Interfaces;
using Lodestone.Domain.Entities;
using Lodestone.Domain.Enums;

namespace Lodestone.Application.Services;

public class BookingService : IBookingService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IUnitOfWork _unitOfWork;

    public BookingService(IBookingRepository bookingRepository, IUnitOfWork unitOfWork)
    {
        _bookingRepository = bookingRepository;
        _unitOfWork        = unitOfWork;
    }

    public async Task<BookingDto> CreateBookingAsync(
        int studentProfileId, CreateBookingDto dto, CancellationToken cancellationToken = default)
    {
        var booking = new CounselorBooking
        {
            StudentProfileId   = studentProfileId,
            CounselorProfileId = dto.CounselorProfileId,
            AvailabilitySlotId = dto.AvailabilitySlotId,
            ScheduledForUtc    = dto.ScheduledForUtc,
            Notes              = dto.Notes?.Trim(),
            Status             = BookingStatus.Requested,
            CreatedAtUtc       = DateTime.UtcNow,
        };

        await _bookingRepository.AddAsync(booking, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return MapToDto(booking);
    }

    public async Task<IReadOnlyList<BookingDto>> GetStudentBookingsAsync(
        int studentProfileId, CancellationToken cancellationToken = default)
    {
        var bookings = await _bookingRepository.GetByStudentIdAsync(studentProfileId, cancellationToken);
        return bookings.Select(MapToDto).ToList().AsReadOnly();
    }

    public async Task<IReadOnlyList<BookingDto>> GetUpcomingAsync(
        int counselorProfileId, CancellationToken cancellationToken = default)
    {
        var bookings = await _bookingRepository.GetByCounselorIdAsync(counselorProfileId, cancellationToken);
        return bookings.Select(MapToDto).ToList().AsReadOnly();
    }

    public async Task<IReadOnlyList<CounselorSummaryDto>> GetCounselorsAsync(
        CancellationToken cancellationToken = default)
    {
        var counselors = await _bookingRepository.GetAllCounselorsAsync(cancellationToken);
        return counselors
            .Select(c => new CounselorSummaryDto(
                c.Id,
                c.Specialization != null ? $"Counselor — {c.Specialization}" : $"Counselor #{c.Id}"))
            .ToList()
            .AsReadOnly();
    }

    public async Task CancelAsync(int bookingId, CancellationToken cancellationToken = default)
    {
        var booking = await _bookingRepository.GetByIdAsync(bookingId, cancellationToken)
            ?? throw new InvalidOperationException($"Booking {bookingId} not found.");

        booking.Status = BookingStatus.Cancelled;
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private static BookingDto MapToDto(CounselorBooking b)
        => new(b.Id, b.CounselorProfileId, b.StudentProfileId, b.ScheduledForUtc, b.Status);
}
