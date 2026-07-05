namespace Lodestone.Application.Interfaces;

/// <summary>Transaction boundary over the repositories (implemented in Infrastructure).</summary>
public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
