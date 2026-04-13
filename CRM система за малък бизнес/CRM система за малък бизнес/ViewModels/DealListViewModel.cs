using CrmSmallBusiness.DTOs;

namespace CrmSmallBusiness.ViewModels;

public class DealListViewModel
{
    public IReadOnlyCollection<DealDto> Deals { get; init; } = [];
}
