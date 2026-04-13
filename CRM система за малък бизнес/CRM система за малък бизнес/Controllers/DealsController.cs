using CrmSmallBusiness.Domain.Constants;
using CrmSmallBusiness.Services.Interfaces;
using CrmSmallBusiness.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CrmSmallBusiness.Controllers;

[Authorize(Roles = $"{AppRoles.Admin},{AppRoles.Manager}")]
public class DealsController(IDealService dealService) : Controller
{
    public async Task<IActionResult> Index()
    {
        var model = new DealListViewModel
        {
            Deals = await dealService.GetAllAsync()
        };

        return View(model);
    }
}
