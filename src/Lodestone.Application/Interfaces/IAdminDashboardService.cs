using Lodestone.Application.DTOs.Admin;

namespace Lodestone.Application.Interfaces;

public interface IAdminDashboardService
{
    Task<AdminShellDto> GetShellAsync(CancellationToken cancellationToken = default);
    Task<AdminDashboardDto> GetDashboardAsync(CancellationToken cancellationToken = default);
    Task<AdminSectionPageDto> GetSectionAsync(AdminSectionType section, CancellationToken cancellationToken = default);
}
