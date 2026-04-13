using Microsoft.AspNetCore.Identity;

namespace CrmSmallBusiness.Data;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;
}
