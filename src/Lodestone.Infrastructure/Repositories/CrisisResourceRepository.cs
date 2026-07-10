using Lodestone.Application.Interfaces;
using Lodestone.Domain.Entities;
using Lodestone.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Lodestone.Infrastructure.Repositories;

/// <summary>CrisisResource-specific queries.</summary>
public class CrisisResourceRepository : GenericRepository<CrisisResource>, ICrisisResourceRepository
{
    public CrisisResourceRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IReadOnlyList<CrisisResource>> GetActiveOrderedAsync(CancellationToken cancellationToken = default)
        => await Set
            .Where(r => !r.IsDeleted)
            .OrderByDescending(r => r.IsEmergency)
            .ThenBy(r => r.DisplayOrder)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
}
