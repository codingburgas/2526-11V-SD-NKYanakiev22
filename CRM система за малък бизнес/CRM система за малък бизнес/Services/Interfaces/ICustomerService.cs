using CrmSmallBusiness.DTOs;

namespace CrmSmallBusiness.Services.Interfaces;

public interface ICustomerService
{
    Task<IReadOnlyCollection<CustomerDto>> GetAllAsync();
    Task<IReadOnlyCollection<CustomerDto>> GetTopCustomersAsync(int count);
}
