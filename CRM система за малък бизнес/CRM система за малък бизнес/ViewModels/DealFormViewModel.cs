using System.ComponentModel.DataAnnotations;
using CrmSmallBusiness.Domain.Enums;

namespace CrmSmallBusiness.ViewModels;

public class DealFormViewModel
{
    [Range(1, int.MaxValue)]
    [Display(Name = "Business")]
    public int BusinessId { get; set; }

    [Required]
    [StringLength(160)]
    public string Title { get; set; } = string.Empty;

    [StringLength(160)]
    [Display(Name = "Contact name")]
    public string ContactName { get; set; } = string.Empty;

    [Range(0.01d, 999999999.99d)]
    public decimal Value { get; set; }

    public DealStage Stage { get; set; } = DealStage.Prospecting;

    [DataType(DataType.Date)]
    [Display(Name = "Expected close date")]
    public DateTime ExpectedCloseDate { get; set; } = DateTime.UtcNow.AddDays(30);
}
