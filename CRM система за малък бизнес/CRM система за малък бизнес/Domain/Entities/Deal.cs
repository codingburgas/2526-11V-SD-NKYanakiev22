using System.ComponentModel.DataAnnotations;
using CrmSmallBusiness.Domain.Enums;

namespace CrmSmallBusiness.Domain.Entities;

public class Deal : BaseEntity
{
    [Required]
    [MaxLength(160)]
    public string Title { get; set; } = string.Empty;

    [Range(0, 999999999)]
    public decimal Value { get; set; }

    public DealStage Stage { get; set; } = DealStage.Prospecting;

    public DateTime ExpectedCloseDate { get; set; } = DateTime.UtcNow.AddDays(30);

    public bool IsClosedWon { get; set; }

    public bool IsClosedLost { get; set; }

    public int CustomerId { get; set; }
    public Customer? Customer { get; set; }
}
