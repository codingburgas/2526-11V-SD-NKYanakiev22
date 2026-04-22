namespace CrmSmallBusiness.ViewModels;

public class BusinessInvestorViewModel
{
    public string InvestorName { get; init; } = string.Empty;
    public decimal Amount { get; init; }
    public string Note { get; init; } = string.Empty;
    public DateTime CreatedOnUtc { get; init; }
}
