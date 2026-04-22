using System.ComponentModel.DataAnnotations;

namespace CrmSmallBusiness.Domain.Entities;

public class Business : BaseEntity
{
    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(150)]
    public string OwnerName { get; set; } = string.Empty;

    [MaxLength(150)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [MaxLength(30)]
    [Phone]
    public string PhoneNumber { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Industry { get; set; } = string.Empty;

    [MaxLength(250)]
    public string Address { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;

    [Required]
    public string CreatedByUserId { get; set; } = string.Empty;

    public bool IsSeededExample { get; set; }

    public ICollection<BusinessInvestment> Investments { get; set; } = new List<BusinessInvestment>();
    public ICollection<BusinessDeal> Deals { get; set; } = new List<BusinessDeal>();
}
