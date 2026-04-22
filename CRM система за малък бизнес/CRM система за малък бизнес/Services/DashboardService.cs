using CrmSmallBusiness.Data;
using Microsoft.EntityFrameworkCore;
using CrmSmallBusiness.Services.Interfaces;
using CrmSmallBusiness.ViewModels;

namespace CrmSmallBusiness.Services;

public class DashboardService(
    DemoCrmStore store,
    ApplicationDbContext context,
    IDealService dealService) : IDashboardService
{
    public async Task<DashboardViewModel> GetDashboardAsync()
    {
        var totalCustomers = store.Customers.Count;
        var activeDeals = await dealService.GetActiveDealCountAsync();
        var totalPipelineValue = await dealService.GetActivePipelineValueAsync();
        var conversionRate = await dealService.GetConversionRateAsync();
        var owners = await context.Businesses
            .AsNoTracking()
            .OrderByDescending(business => business.CreatedOnUtc)
            .Take(5)
            .Select(business => new DTOs.BusinessOwnerDto
            {
                OwnerName = business.OwnerName,
                BusinessName = business.Name,
                Email = business.Email,
                Industry = business.Industry
            })
            .ToArrayAsync();

        return new DashboardViewModel
        {
            TotalCustomers = totalCustomers,
            ActiveDeals = activeDeals,
            TotalPipelineValue = totalPipelineValue,
            ConversionRate = conversionRate,
            RegisteredOwners = owners,
            UpcomingDeals = await dealService.GetUpcomingAsync(5)
        };
    }
}
