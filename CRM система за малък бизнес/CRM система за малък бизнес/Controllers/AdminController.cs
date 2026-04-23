using CrmSmallBusiness.Data;
using CrmSmallBusiness.Domain.Constants;
using CrmSmallBusiness.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CrmSmallBusiness.Controllers;

[Authorize(Roles = $"{AppRoles.Admin},{AppRoles.Manager}")]
public class AdminController(UserManager<ApplicationUser> userManager) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Users()
    {
        var items = new List<UserRoleItemViewModel>();

        foreach (var user in userManager.Users.OrderBy(user => user.Email))
        {
            var roles = await userManager.GetRolesAsync(user);
            items.Add(new UserRoleItemViewModel
            {
                UserId = user.Id,
                FullName = user.FullName,
                Email = user.Email ?? string.Empty,
                CurrentRole = roles.FirstOrDefault() ?? AppRoles.User
            });
        }

        return View(new UserManagementViewModel
        {
            Users = items,
            CanAssignRoles = User.IsInRole(AppRoles.Admin)
        });
    }

    [Authorize(Roles = AppRoles.Admin)]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateRole(UpdateUserRoleViewModel model)
    {
        if (!ModelState.IsValid || !AppRoles.All.Contains(model.Role))
        {
            return RedirectToAction(nameof(Users));
        }

        var user = await userManager.FindByIdAsync(model.UserId);
        if (user is null)
        {
            return RedirectToAction(nameof(Users));
        }

        var existingRoles = await userManager.GetRolesAsync(user);
        if (existingRoles.Count > 0)
        {
            await userManager.RemoveFromRolesAsync(user, existingRoles);
        }

        await userManager.AddToRoleAsync(user, model.Role);
        return RedirectToAction(nameof(Users));
    }
}
