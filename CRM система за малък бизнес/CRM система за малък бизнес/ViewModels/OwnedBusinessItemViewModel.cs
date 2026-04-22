namespace CrmSmallBusiness.ViewModels;

public class OwnedBusinessItemViewModel
{
    public int Id { get; init; }
    public BusinessFormViewModel Form { get; init; } = new();
    public bool IsSeededExample { get; init; }
    public decimal TotalInvested { get; init; }
    public int InvestorCount { get; init; }
    public IReadOnlyCollection<BusinessInvestorViewModel> Investors { get; init; } = [];
}
