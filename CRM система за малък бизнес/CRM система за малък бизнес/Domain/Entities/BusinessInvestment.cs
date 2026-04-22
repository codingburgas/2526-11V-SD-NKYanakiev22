using System.ComponentModel.DataAnnotations;

namespace CrmSmallBusiness.Domain.Entities;

public class BusinessInvestment : BaseEntity
{
    public int BusinessId { get; set; }
    public Business? Business { get; set; }

    [Required]
    public string InvestorUserId { get; set; } = string.Empty;

    [Required]
    [MaxLength(150)]
    public string InvestorName { get; set; } = string.Empty;

    [Range(0.01d, 999999999.99d)]
    public decimal Amount { get; set; }

    [MaxLength(500)]
    public string Note { get; set; } = string.Empty;
}
