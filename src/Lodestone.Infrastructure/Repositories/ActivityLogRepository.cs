using Lodestone.Infrastructure.Data;

namespace Lodestone.Infrastructure.Repositories;

/// <summary>ActivityLog-specific queries. Extend with entity-tailored read/write methods.</summary>
public class ActivityLogRepository
{
    private readonly ApplicationDbContext _context;

    public ActivityLogRepository(ApplicationDbContext context) => _context = context;
}
