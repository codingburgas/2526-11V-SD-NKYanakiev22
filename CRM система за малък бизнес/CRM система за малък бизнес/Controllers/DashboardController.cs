using CrmSmallBusiness.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CrmSmallBusiness.Controllers;

[Authorize]
public class DashboardController(IDashboardService dashboardService) : Controller
{
    public async Task<IActionResult> Index()
    {
        var model = await dashboardService.GetDashboardAsync();
        return View(model);
    }
}
