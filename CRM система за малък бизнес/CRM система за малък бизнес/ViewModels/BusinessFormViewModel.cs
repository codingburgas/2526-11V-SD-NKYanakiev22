using System.ComponentModel.DataAnnotations;

namespace CrmSmallBusiness.ViewModels;

public class BusinessFormViewModel
{
    [Required]
    [StringLength(150)]
    [Display(Name = "Business name")]
    public string Name { get; set; } = string.Empty;

    [StringLength(150)]
    [Display(Name = "Owner name")]
    public string OwnerName { get; set; } = string.Empty;

    [EmailAddress]
    [StringLength(150)]
    public string Email { get; set; } = string.Empty;

    [Phone]
    [StringLength(30)]
    [Display(Name = "Phone number")]
    public string PhoneNumber { get; set; } = string.Empty;

    [StringLength(100)]
    public string Industry { get; set; } = string.Empty;

    [StringLength(250)]
    public string Address { get; set; } = string.Empty;

    [StringLength(1000)]
    public string Description { get; set; } = string.Empty;
}
