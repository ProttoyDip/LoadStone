using Lodestone.Application.Interfaces;
using Lodestone.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Lodestone.Infrastructure.Repositories;

public class StudentProfileRepository : IStudentProfileRepository
{
    private readonly ApplicationDbContext _context;

    public StudentProfileRepository(ApplicationDbContext context) => _context = context;

    public Task<int?> GetIdByUserIdAsync(string userId, CancellationToken cancellationToken = default)
        => _context.StudentProfiles
            .AsNoTracking()
            .Where(profile => profile.UserId == userId)
            .Select(profile => (int?)profile.Id)
            .SingleOrDefaultAsync(cancellationToken);
}
