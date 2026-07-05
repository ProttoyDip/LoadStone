using Lodestone.Infrastructure.Data;

namespace Lodestone.Infrastructure.Repositories;

/// <summary>CounselorQueue-specific queries. Extend with entity-tailored read/write methods.</summary>
public class CounselorQueueRepository
{
    private readonly ApplicationDbContext _context;

    public CounselorQueueRepository(ApplicationDbContext context) => _context = context;
}
