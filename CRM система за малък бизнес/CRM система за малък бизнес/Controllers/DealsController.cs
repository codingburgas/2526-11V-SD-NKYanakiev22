using CrmSmallBusiness.Services.Interfaces;
using CrmSmallBusiness.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CrmSmallBusiness.Controllers;

[Authorize]
public class DealsController(IDealService dealService, IBusinessService businessService) : Controller
{
    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Challenge();
        }

        if (!await businessService.HasOwnedBusinessAsync(userId))
        {
            TempData["BusinessError"] = "Register a business first to access deals.";
            return RedirectToAction("Mine", "Businesses");
        }

        var model = new DealListViewModel
        {
            Deals = await dealService.GetAllAsync(userId),
            OwnedBusinesses = await businessService.GetOwnedOptionsAsync(userId)
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(DealListViewModel model)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Challenge();
        }

        if (!await businessService.HasOwnedBusinessAsync(userId))
        {
            TempData["BusinessError"] = "Register a business first to access deals.";
            return RedirectToAction("Mine", "Businesses");
        }

        if (!ModelState.IsValid)
        {
            return View("Index", new DealListViewModel
            {
                CreateForm = model.CreateForm,
                OwnedBusinesses = await businessService.GetOwnedOptionsAsync(userId),
                Deals = await dealService.GetAllAsync(userId)
            });
        }

        try
        {
            await dealService.CreateAsync(model.CreateForm, userId);
            TempData["DealMessage"] = "Deal created successfully.";
        }
        catch (InvalidOperationException exception)
        {
            TempData["DealError"] = exception.Message;
        }

        return RedirectToAction(nameof(Index));
    }
}
