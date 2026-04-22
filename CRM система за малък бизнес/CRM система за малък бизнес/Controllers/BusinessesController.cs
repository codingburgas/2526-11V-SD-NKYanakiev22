using System.Security.Claims;
using CrmSmallBusiness.Services.Interfaces;
using CrmSmallBusiness.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CrmSmallBusiness.Controllers;

[Authorize]
public class BusinessesController(IBusinessService businessService) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Challenge();
        }

        return View(new BusinessIndexViewModel
        {
            Businesses = await businessService.GetCatalogAsync(userId)
        });
    }

    [HttpGet]
    public async Task<IActionResult> Mine()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Challenge();
        }

        return View(new MyBusinessesViewModel
        {
            Businesses = await businessService.GetOwnedAsync(userId)
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(MyBusinessesViewModel model)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!ModelState.IsValid)
        {
            return View("Mine", new MyBusinessesViewModel
            {
                CreateForm = model.CreateForm,
                Businesses = await businessService.GetOwnedAsync(currentUserId ?? string.Empty)
            });
        }

        if (string.IsNullOrWhiteSpace(currentUserId))
        {
            return Challenge();
        }

        var userDisplayName = User.Identity?.Name ?? "User";
        await businessService.CreateAsync(model.CreateForm, currentUserId, userDisplayName);

        TempData["BusinessMessage"] = "Business registered successfully.";
        return RedirectToAction(nameof(Mine));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(int businessId, BusinessFormViewModel model)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(currentUserId))
        {
            return Challenge();
        }

        if (!ModelState.IsValid)
        {
            TempData["BusinessError"] = "Enter valid business information before saving changes.";
            return RedirectToAction(nameof(Mine));
        }

        try
        {
            await businessService.UpdateAsync(businessId, model, currentUserId);
            TempData["BusinessMessage"] = "Business information updated successfully.";
        }
        catch (InvalidOperationException)
        {
            TempData["BusinessError"] = "The selected business could not be found.";
        }

        return RedirectToAction(nameof(Mine));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Invest(int businessId, BusinessInvestmentFormViewModel model)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Challenge();
        }

        if (!ModelState.IsValid)
        {
            TempData["BusinessError"] = "Enter a valid investment amount.";
            return RedirectToAction(nameof(Index));
        }

        var investorName = User.Identity?.Name ?? "User";

        try
        {
            await businessService.InvestAsync(businessId, model, userId, investorName);
            TempData["BusinessMessage"] = "Investment submitted successfully.";
        }
        catch (InvalidOperationException exception)
        {
            TempData["BusinessError"] = exception.Message;
        }

        return RedirectToAction(nameof(Index));
    }
}
