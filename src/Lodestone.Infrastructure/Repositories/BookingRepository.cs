using Lodestone.Infrastructure.Data;

namespace Lodestone.Infrastructure.Repositories;

/// <summary>Booking-specific queries. Extend with entity-tailored read/write methods.</summary>
public class BookingRepository
{
    private readonly ApplicationDbContext _context;

    public BookingRepository(ApplicationDbContext context) => _context = context;
}
