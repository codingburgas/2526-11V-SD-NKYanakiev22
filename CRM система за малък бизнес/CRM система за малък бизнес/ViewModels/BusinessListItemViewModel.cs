namespace CrmSmallBusiness.ViewModels;

public class BusinessListItemViewModel
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string OwnerName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string PhoneNumber { get; init; } = string.Empty;
    public string Industry { get; init; } = string.Empty;
    public string Address { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string CreatedBy { get; init; } = string.Empty;
    public DateTime CreatedOnUtc { get; init; }
    public bool IsSeededExample { get; init; }
    public bool IsOwnedByCurrentUser { get; init; }
    public decimal TotalInvested { get; init; }
    public int InvestorCount { get; init; }
    public IReadOnlyCollection<BusinessInvestorViewModel> Investors { get; init; } = [];
}
