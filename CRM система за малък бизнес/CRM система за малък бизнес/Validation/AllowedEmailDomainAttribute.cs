using System.ComponentModel.DataAnnotations;

namespace CrmSmallBusiness.Validation;

public class AllowedEmailDomainAttribute : ValidationAttribute
{
    private static readonly HashSet<string> AllowedDomains =
    [
        "abv.bg",
        "gmail.com",
        "hotmail.com",
        "icloud.com",
        "outlook.com",
        "yahoo.com"
    ];

    public AllowedEmailDomainAttribute()
    {
        ErrorMessage = "Use a valid email provider like gmail.com, abv.bg, outlook.com or yahoo.com.";
    }

    public override bool IsValid(object? value)
    {
        if (value is not string email || string.IsNullOrWhiteSpace(email))
        {
            return true;
        }

        var atIndex = email.LastIndexOf('@');
        if (atIndex < 0 || atIndex == email.Length - 1)
        {
            return false;
        }

        var domain = email[(atIndex + 1)..].Trim().ToLowerInvariant();
        return AllowedDomains.Contains(domain);
    }
}
