using Lodestone.Domain.Entities;

namespace Lodestone.Application.Interfaces;

/// <summary>Booking-specific queries. Implemented in Infrastructure.</summary>
public interface IBookingRepository
{
    Task<IReadOnlyList<CounselorBooking>> GetByStudentIdAsync(int studentProfileId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<CounselorBooking>> GetByCounselorIdAsync(int counselorProfileId, CancellationToken cancellationToken = default);
    Task<CounselorBooking?> GetByIdAsync(int bookingId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<CounselorAvailabilitySlot>> GetAvailableSlotsAsync(int counselorProfileId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<CounselorProfile>> GetAllCounselorsAsync(CancellationToken cancellationToken = default);
    Task AddAsync(CounselorBooking booking, CancellationToken cancellationToken = default);
}
