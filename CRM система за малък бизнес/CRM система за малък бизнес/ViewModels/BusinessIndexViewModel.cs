namespace CrmSmallBusiness.ViewModels;

public class BusinessIndexViewModel
{
    public IReadOnlyCollection<BusinessListItemViewModel> Businesses { get; init; } = [];
}
