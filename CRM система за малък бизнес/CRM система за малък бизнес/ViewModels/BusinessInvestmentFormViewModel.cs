using System.ComponentModel.DataAnnotations;

namespace CrmSmallBusiness.ViewModels;

public class BusinessInvestmentFormViewModel
{
    [Range(0.01d, 999999999.99d)]
    [Display(Name = "Investment amount")]
    public decimal Amount { get; set; }

    [StringLength(500)]
    [Display(Name = "Message")]
    public string Note { get; set; } = string.Empty;
}
