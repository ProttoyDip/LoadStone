using Lodestone.Application.DTOs.Admin;

namespace Lodestone.Web.ViewModels.Admin;

public class AdminSectionViewModel
{
    public AdminSectionViewModel(AdminSectionPageDto page) => Page = page;

    public AdminSectionPageDto Page { get; }
}
