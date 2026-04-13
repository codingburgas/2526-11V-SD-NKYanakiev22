using System.ComponentModel.DataAnnotations;
using CrmSmallBusiness.Validation;

namespace CrmSmallBusiness.ViewModels;

public class LoginViewModel
{
    [Required]
    [EmailAddress]
    [AllowedEmailDomain]
    public string Email { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "Remember me")]
    public bool RememberMe { get; set; }
}
