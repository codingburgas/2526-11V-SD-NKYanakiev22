namespace CrmSmallBusiness.ViewModels;

public class MyBusinessesViewModel
{
    public BusinessFormViewModel CreateForm { get; init; } = new();
    public IReadOnlyCollection<OwnedBusinessItemViewModel> Businesses { get; init; } = [];
}
