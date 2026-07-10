namespace Lodestone.Application.Interfaces;

/// <summary>Resolves student profile identity without exposing persistence details to Web.</summary>
public interface IStudentProfileRepository
{
    Task<int?> GetIdByUserIdAsync(string userId, CancellationToken cancellationToken = default);
}
