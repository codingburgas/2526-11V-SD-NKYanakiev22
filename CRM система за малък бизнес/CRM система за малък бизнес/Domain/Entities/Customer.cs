using System.ComponentModel.DataAnnotations;

namespace CrmSmallBusiness.Domain.Entities;

public class Customer : BaseEntity
{
    [Required]
    [MaxLength(80)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(80)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [MaxLength(150)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [MaxLength(30)]
    [Phone]
    public string PhoneNumber { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Position { get; set; } = string.Empty;

    public int CompanyId { get; set; }
    public Company? Company { get; set; }

    public ICollection<Deal> Deals { get; set; } = new List<Deal>();
}
