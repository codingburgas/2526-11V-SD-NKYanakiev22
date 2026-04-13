using CrmSmallBusiness.ViewModels;

namespace CrmSmallBusiness.Services.Interfaces;

public interface IDashboardService
{
    Task<DashboardViewModel> GetDashboardAsync();
}
