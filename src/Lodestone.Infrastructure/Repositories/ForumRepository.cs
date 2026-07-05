using Lodestone.Infrastructure.Data;

namespace Lodestone.Infrastructure.Repositories;

/// <summary>Forum-specific queries. Extend with entity-tailored read/write methods.</summary>
public class ForumRepository
{
    private readonly ApplicationDbContext _context;

    public ForumRepository(ApplicationDbContext context) => _context = context;
}
