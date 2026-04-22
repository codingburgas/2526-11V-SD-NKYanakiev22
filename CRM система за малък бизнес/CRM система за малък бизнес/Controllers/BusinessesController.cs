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
            Businesses = await businessService.GetAllAsync(userId)
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(BusinessIndexViewModel model)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!ModelState.IsValid)
        {
            return View("Index", new BusinessIndexViewModel
            {
                Form = model.Form,
                Businesses = await businessService.GetAllAsync(currentUserId ?? string.Empty)
            });
        }

        if (string.IsNullOrWhiteSpace(currentUserId))
        {
            return Challenge();
        }

        var userDisplayName = User.Identity?.Name ?? "User";
        await businessService.CreateAsync(model.Form, currentUserId, userDisplayName);

        TempData["BusinessMessage"] = "Business registered successfully.";
        return RedirectToAction(nameof(Index));
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
        catch (InvalidOperationException)
        {
            TempData["BusinessError"] = "The selected business could not be found.";
        }

        return RedirectToAction(nameof(Index));
    }
}
