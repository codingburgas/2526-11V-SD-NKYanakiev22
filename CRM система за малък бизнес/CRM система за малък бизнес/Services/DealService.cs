using CrmSmallBusiness.Data;
using CrmSmallBusiness.DTOs;
using CrmSmallBusiness.Domain.Enums;
using CrmSmallBusiness.Services.Interfaces;

namespace CrmSmallBusiness.Services;

public class DealService(DemoCrmStore store) : IDealService
{
    public Task<IReadOnlyCollection<DealDto>> GetAllAsync()
    {
        IReadOnlyCollection<DealDto> deals = store.Deals
            .OrderByDescending(deal => deal.Value)
            .Select(MapDeal)
            .ToArray();

        return Task.FromResult(deals);
    }

    public Task<IReadOnlyCollection<DealDto>> GetUpcomingAsync(int count)
    {
        IReadOnlyCollection<DealDto> deals = store.Deals
            .Where(deal => deal.Stage != DealStage.ClosedLost && deal.Stage != DealStage.ClosedWon)
            .OrderBy(deal => deal.ExpectedCloseDate)
            .Take(count)
            .Select(MapDeal)
            .ToArray();

        return Task.FromResult(deals);
    }

    private static DealDto MapDeal(Domain.Entities.Deal deal)
    {
        return new DealDto
        {
            Id = deal.Id,
            Title = deal.Title,
            CustomerName = deal.Customer != null
                ? $"{deal.Customer.FirstName} {deal.Customer.LastName}"
                : "Unknown customer",
            Stage = deal.Stage,
            Value = deal.Value,
            ExpectedCloseDate = deal.ExpectedCloseDate
        };
    }
}
