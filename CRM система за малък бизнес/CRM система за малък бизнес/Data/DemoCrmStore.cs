using CrmSmallBusiness.Domain.Entities;
using CrmSmallBusiness.Domain.Enums;

namespace CrmSmallBusiness.Data;

public class DemoCrmStore
{
    public DemoCrmStore()
    {
        var vitosha = new Company
        {
            Id = 1,
            Name = "Vitosha Trade",
            Industry = "Retail",
            Email = "contact@vitoshatrade.bg",
            PhoneNumber = "+359888000111"
        };

        var danube = new Company
        {
            Id = 2,
            Name = "Danube Logistics",
            Industry = "Logistics",
            Email = "office@danubelogistics.bg",
            PhoneNumber = "+359887000222"
        };

        Companies = [vitosha, danube];

        var ivan = new Customer
        {
            Id = 1,
            FirstName = "Ivan",
            LastName = "Petrov",
            Email = "ivan.petrov@vitoshatrade.bg",
            PhoneNumber = "+359888123456",
            Position = "Sales Manager",
            CompanyId = vitosha.Id,
            Company = vitosha
        };

        var maria = new Customer
        {
            Id = 2,
            FirstName = "Maria",
            LastName = "Georgieva",
            Email = "maria.georgieva@danubelogistics.bg",
            PhoneNumber = "+359889654321",
            Position = "Operations Lead",
            CompanyId = danube.Id,
            Company = danube
        };

        Customers = [ivan, maria];
        vitosha.Customers.Add(ivan);
        danube.Customers.Add(maria);

        var supportDeal = new Deal
        {
            Id = 1,
            Title = "Annual support contract",
            Value = 12000m,
            Stage = DealStage.Negotiation,
            ExpectedCloseDate = DateTime.UtcNow.AddDays(12),
            CustomerId = ivan.Id,
            Customer = ivan
        };

        var fleetDeal = new Deal
        {
            Id = 2,
            Title = "Fleet optimization package",
            Value = 18500m,
            Stage = DealStage.Proposal,
            ExpectedCloseDate = DateTime.UtcNow.AddDays(21),
            CustomerId = maria.Id,
            Customer = maria
        };

        var onboardingDeal = new Deal
        {
            Id = 3,
            Title = "Completed onboarding",
            Value = 9000m,
            Stage = DealStage.ClosedWon,
            ExpectedCloseDate = DateTime.UtcNow.AddDays(-8),
            CustomerId = ivan.Id,
            Customer = ivan,
            IsClosedWon = true
        };

        Deals = [supportDeal, fleetDeal, onboardingDeal];
        ivan.Deals.Add(supportDeal);
        ivan.Deals.Add(onboardingDeal);
        maria.Deals.Add(fleetDeal);
    }

    public IReadOnlyCollection<Company> Companies { get; }
    public IReadOnlyCollection<Customer> Customers { get; }
    public IReadOnlyCollection<Deal> Deals { get; }
}
