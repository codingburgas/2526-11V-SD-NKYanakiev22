using CrmSmallBusiness.DTOs;

namespace CrmSmallBusiness.ViewModels;

public class DashboardViewModel
{
    public int TotalCustomers { get; init; }
    public int ActiveDeals { get; init; }
    public decimal TotalPipelineValue { get; init; }
    public decimal ConversionRate { get; init; }
    public IReadOnlyCollection<BusinessOwnerDto> RegisteredOwners { get; init; } = [];
    public IReadOnlyCollection<DealDto> UpcomingDeals { get; init; } = [];
}
