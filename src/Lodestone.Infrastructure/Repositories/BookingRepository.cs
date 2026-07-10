using Lodestone.Application.Interfaces;
using Lodestone.Domain.Entities;
using Lodestone.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Lodestone.Infrastructure.Repositories;

/// <summary>Booking-specific queries: student bookings, counselor bookings, slots, counselor list.</summary>
public class BookingRepository : GenericRepository<CounselorBooking>, IBookingRepository
{
    public BookingRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IReadOnlyList<CounselorBooking>> GetByStudentIdAsync(
        int studentProfileId, CancellationToken cancellationToken = default)
        => await Set
            .Include(b => b.CounselorProfile)
            .Where(b => b.StudentProfileId == studentProfileId)
            .OrderByDescending(b => b.ScheduledForUtc)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<CounselorBooking>> GetByCounselorIdAsync(
        int counselorProfileId, CancellationToken cancellationToken = default)
        => await Set
            .Include(b => b.StudentProfile)
            .Where(b => b.CounselorProfileId == counselorProfileId
                     && b.ScheduledForUtc >= DateTime.UtcNow)
            .OrderBy(b => b.ScheduledForUtc)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

    public async Task<CounselorBooking?> GetByIdAsync(int bookingId, CancellationToken cancellationToken = default)
        => await Set
            .Include(b => b.CounselorProfile)
            .Include(b => b.StudentProfile)
            .FirstOrDefaultAsync(b => b.Id == bookingId, cancellationToken);

    public async Task<IReadOnlyList<CounselorAvailabilitySlot>> GetAvailableSlotsAsync(
        int counselorProfileId, CancellationToken cancellationToken = default)
        => await Context.CounselorAvailabilitySlots
            .Where(s => s.CounselorProfileId == counselorProfileId
                     && !s.IsBooked
                     && s.StartUtc > DateTime.UtcNow)
            .OrderBy(s => s.StartUtc)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<CounselorProfile>> GetAllCounselorsAsync(
        CancellationToken cancellationToken = default)
        => await Context.CounselorProfiles
            .AsNoTracking()
            .ToListAsync(cancellationToken);

    public async Task AddAsync(CounselorBooking booking, CancellationToken cancellationToken = default)
        => await Set.AddAsync(booking, cancellationToken);
}
