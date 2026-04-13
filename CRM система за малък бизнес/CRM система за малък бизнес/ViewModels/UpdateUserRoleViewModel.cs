using System.ComponentModel.DataAnnotations;

namespace CrmSmallBusiness.ViewModels;

public class UpdateUserRoleViewModel
{
    [Required]
    public string UserId { get; set; } = string.Empty;

    [Required]
    public string Role { get; set; } = string.Empty;
}
