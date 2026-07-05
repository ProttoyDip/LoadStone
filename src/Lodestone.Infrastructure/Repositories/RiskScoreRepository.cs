using Lodestone.Infrastructure.Data;

namespace Lodestone.Infrastructure.Repositories;

/// <summary>RiskScore-specific queries. Extend with entity-tailored read/write methods.</summary>
public class RiskScoreRepository
{
    private readonly ApplicationDbContext _context;

    public RiskScoreRepository(ApplicationDbContext context) => _context = context;
}
