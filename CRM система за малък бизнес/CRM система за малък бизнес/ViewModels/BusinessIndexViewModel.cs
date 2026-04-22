namespace CrmSmallBusiness.ViewModels;

public class BusinessIndexViewModel
{
    public BusinessFormViewModel Form { get; init; } = new();
    public IReadOnlyCollection<BusinessListItemViewModel> Businesses { get; init; } = [];
}
