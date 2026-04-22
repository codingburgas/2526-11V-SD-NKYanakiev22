using CrmSmallBusiness.DTOs;
using CrmSmallBusiness.ViewModels;

namespace CrmSmallBusiness.Services.Interfaces;

public interface IDealService
{
    Task<IReadOnlyCollection<DealDto>> GetAllAsync(string currentUserId);
    Task<int> GetActiveDealCountAsync();
    Task<decimal> GetActivePipelineValueAsync();
    Task<decimal> GetConversionRateAsync();
    Task CreateAsync(DealFormViewModel model, string currentUserId);
    Task<IReadOnlyCollection<DealDto>> GetUpcomingAsync(int count);
}
