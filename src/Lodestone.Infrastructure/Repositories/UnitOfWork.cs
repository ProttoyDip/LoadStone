using Lodestone.Application.Interfaces;
using Lodestone.Infrastructure.Data;

namespace Lodestone.Infrastructure.Repositories;

/// <summary>Implements the Application-owned <see cref="IUnitOfWork"/> over EF Core.</summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public UnitOfWork(ApplicationDbContext context) => _context = context;

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => _context.SaveChangesAsync(cancellationToken);
}
