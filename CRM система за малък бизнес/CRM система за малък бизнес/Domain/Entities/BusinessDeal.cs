using System.ComponentModel.DataAnnotations;
using CrmSmallBusiness.Domain.Enums;

namespace CrmSmallBusiness.Domain.Entities;

public class BusinessDeal : BaseEntity
{
    public int BusinessId { get; set; }
    public Business? Business { get; set; }

    [Required]
    [MaxLength(160)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(160)]
    public string ContactName { get; set; } = string.Empty;

    [Range(0, 999999999)]
    public decimal Value { get; set; }

    public DealStage Stage { get; set; } = DealStage.Prospecting;

    public DateTime ExpectedCloseDate { get; set; } = DateTime.UtcNow.AddDays(30);
}
