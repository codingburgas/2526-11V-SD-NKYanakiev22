using CrmSmallBusiness.Data;
using CrmSmallBusiness.Domain.Enums;
using CrmSmallBusiness.Services.Interfaces;
using CrmSmallBusiness.ViewModels;

namespace CrmSmallBusiness.Services;

public class DashboardService(
    DemoCrmStore store,
    ICustomerService customerService,
    IDealService dealService) : IDashboardService
{
    public async Task<DashboardViewModel> GetDashboardAsync()
    {
        var totalCustomers = store.Customers.Count;
        var activeDeals = store.Deals.Count(deal =>
            deal.Stage != DealStage.ClosedWon && deal.Stage != DealStage.ClosedLost);

        var totalPipelineValue = store.Deals
            .Where(deal => deal.Stage != DealStage.ClosedWon && deal.Stage != DealStage.ClosedLost)
            .Sum(deal => deal.Value);

        var totalDeals = store.Deals.Count;
        var wonDeals = store.Deals.Count(deal => deal.Stage == DealStage.ClosedWon || deal.IsClosedWon);
        var conversionRate = totalDeals == 0 ? 0 : Math.Round((decimal)wonDeals / totalDeals * 100, 2);

        return new DashboardViewModel
        {
            TotalCustomers = totalCustomers,
            ActiveDeals = activeDeals,
            TotalPipelineValue = totalPipelineValue,
            ConversionRate = conversionRate,
            TopCustomers = await customerService.GetTopCustomersAsync(5),
            UpcomingDeals = await dealService.GetUpcomingAsync(5)
        };
    }
}
