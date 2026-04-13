using CrmSmallBusiness.DTOs;

namespace CrmSmallBusiness.ViewModels;

public class CustomerListViewModel
{
    public IReadOnlyCollection<CustomerDto> Customers { get; init; } = [];
}
