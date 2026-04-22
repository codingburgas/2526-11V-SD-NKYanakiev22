using CrmSmallBusiness.Data;
using CrmSmallBusiness.DTOs;
using CrmSmallBusiness.Domain.Entities;
using CrmSmallBusiness.Domain.Enums;
using CrmSmallBusiness.Services.Interfaces;
using CrmSmallBusiness.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CrmSmallBusiness.Services;

public class DealService(ApplicationDbContext context) : IDealService
{
    public async Task<IReadOnlyCollection<DealDto>> GetAllAsync(string currentUserId)
    {
        return await context.BusinessDeals
            .AsNoTracking()
            .Include(deal => deal.Business)
            .Where(deal => deal.Business != null && deal.Business.CreatedByUserId == currentUserId)
            .OrderByDescending(deal => deal.CreatedOnUtc)
            .Select(deal => new DealDto
            {
                Id = deal.Id,
                Title = deal.Title,
                CustomerName = string.IsNullOrWhiteSpace(deal.ContactName) ? deal.Business!.Name : deal.ContactName,
                Stage = deal.Stage,
                Value = deal.Value,
                ExpectedCloseDate = deal.ExpectedCloseDate
            })
            .ToArrayAsync();
    }

    public async Task<int> GetActiveDealCountAsync()
    {
        return await context.BusinessDeals.CountAsync(deal =>
            deal.Stage != DealStage.ClosedWon && deal.Stage != DealStage.ClosedLost);
    }

    public async Task<decimal> GetActivePipelineValueAsync()
    {
        return await context.BusinessDeals
            .Where(deal => deal.Stage != DealStage.ClosedWon && deal.Stage != DealStage.ClosedLost)
            .SumAsync(deal => deal.Value);
    }

    public async Task<decimal> GetConversionRateAsync()
    {
        var totalDeals = await context.BusinessDeals.CountAsync();
        if (totalDeals == 0)
        {
            return 0;
        }

        var wonDeals = await context.BusinessDeals.CountAsync(deal => deal.Stage == DealStage.ClosedWon);
        return Math.Round((decimal)wonDeals / totalDeals * 100, 2);
    }

    public async Task CreateAsync(DealFormViewModel model, string currentUserId)
    {
        var business = await context.Businesses
            .FirstOrDefaultAsync(item => item.Id == model.BusinessId && item.CreatedByUserId == currentUserId);

        if (business is null)
        {
            throw new InvalidOperationException("You can only create deals for businesses you own.");
        }

        context.BusinessDeals.Add(new BusinessDeal
        {
            BusinessId = model.BusinessId,
            Title = model.Title.Trim(),
            ContactName = model.ContactName.Trim(),
            Value = model.Value,
            Stage = model.Stage,
            ExpectedCloseDate = model.ExpectedCloseDate,
            CreatedOnUtc = DateTime.UtcNow
        });

        await context.SaveChangesAsync();
    }

    public async Task<IReadOnlyCollection<DealDto>> GetUpcomingAsync(int count)
    {
        return await context.BusinessDeals
            .AsNoTracking()
            .Include(deal => deal.Business)
            .Where(deal => deal.Stage != DealStage.ClosedLost && deal.Stage != DealStage.ClosedWon)
            .OrderBy(deal => deal.ExpectedCloseDate)
            .Take(count)
            .Select(deal => new DealDto
            {
                Id = deal.Id,
                Title = deal.Title,
                CustomerName = string.IsNullOrWhiteSpace(deal.ContactName) ? deal.Business!.Name : deal.ContactName,
                Stage = deal.Stage,
                Value = deal.Value,
                ExpectedCloseDate = deal.ExpectedCloseDate
            })
            .ToArrayAsync();
    }
}
