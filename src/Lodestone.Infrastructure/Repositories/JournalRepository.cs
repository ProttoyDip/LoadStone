using Lodestone.Infrastructure.Data;

namespace Lodestone.Infrastructure.Repositories;

/// <summary>Journal-specific queries. Extend with entity-tailored read/write methods.</summary>
public class JournalRepository
{
    private readonly ApplicationDbContext _context;

    public JournalRepository(ApplicationDbContext context) => _context = context;
}
