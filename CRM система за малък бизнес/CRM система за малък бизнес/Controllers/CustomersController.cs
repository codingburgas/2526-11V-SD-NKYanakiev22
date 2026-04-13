using CrmSmallBusiness.Domain.Constants;
using CrmSmallBusiness.Services.Interfaces;
using CrmSmallBusiness.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CrmSmallBusiness.Controllers;

[Authorize(Roles = $"{AppRoles.Admin},{AppRoles.Manager},{AppRoles.User}")]
public class CustomersController(ICustomerService customerService) : Controller
{
    public async Task<IActionResult> Index()
    {
        var model = new CustomerListViewModel
        {
            Customers = await customerService.GetAllAsync()
        };

        return View(model);
    }
}
