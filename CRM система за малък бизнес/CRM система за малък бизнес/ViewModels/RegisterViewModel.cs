using System.ComponentModel.DataAnnotations;
using CrmSmallBusiness.Validation;

namespace CrmSmallBusiness.ViewModels;

public class RegisterViewModel
{
    [Required]
    [MaxLength(120)]
    [Display(Name = "Full name")]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [AllowedEmailDomain]
    public string Email { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    [Compare(nameof(Password))]
    [Display(Name = "Confirm password")]
    public string ConfirmPassword { get; set; } = string.Empty;
}
