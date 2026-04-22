using CrmSmallBusiness.DTOs;

namespace CrmSmallBusiness.ViewModels;

public class DealListViewModel
{
    public DealFormViewModel CreateForm { get; init; } = new();
    public IReadOnlyCollection<OwnedBusinessOptionViewModel> OwnedBusinesses { get; init; } = [];
    public IReadOnlyCollection<DealDto> Deals { get; init; } = [];
}
