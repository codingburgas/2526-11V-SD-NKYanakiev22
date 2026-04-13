using CrmSmallBusiness.Data;
using CrmSmallBusiness.DTOs;
using CrmSmallBusiness.Services.Interfaces;

namespace CrmSmallBusiness.Services;

public class CustomerService(DemoCrmStore store) : ICustomerService
{
    public Task<IReadOnlyCollection<CustomerDto>> GetAllAsync()
    {
        IReadOnlyCollection<CustomerDto> customers = store.Customers
            .OrderBy(customer => customer.FirstName)
            .ThenBy(customer => customer.LastName)
            .Select(MapCustomer)
            .ToArray();

        return Task.FromResult(customers);
    }

    public Task<IReadOnlyCollection<CustomerDto>> GetTopCustomersAsync(int count)
    {
        IReadOnlyCollection<CustomerDto> customers = store.Customers
            .OrderByDescending(customer => customer.Deals.Sum(deal => deal.Value))
            .Take(count)
            .Select(MapCustomer)
            .ToArray();

        return Task.FromResult(customers);
    }

    private static CustomerDto MapCustomer(Domain.Entities.Customer customer)
    {
        return new CustomerDto
        {
            Id = customer.Id,
            FullName = $"{customer.FirstName} {customer.LastName}",
            Email = customer.Email,
            CompanyName = customer.Company?.Name ?? "No company",
            Position = customer.Position
        };
    }
}
