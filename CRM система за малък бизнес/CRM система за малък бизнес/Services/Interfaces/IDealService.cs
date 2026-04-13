using CrmSmallBusiness.DTOs;

namespace CrmSmallBusiness.Services.Interfaces;

public interface IDealService
{
    Task<IReadOnlyCollection<DealDto>> GetAllAsync();
    Task<IReadOnlyCollection<DealDto>> GetUpcomingAsync(int count);
}
