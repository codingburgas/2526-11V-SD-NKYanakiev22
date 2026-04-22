using CrmSmallBusiness.Data;
using CrmSmallBusiness.Domain.Entities;
using CrmSmallBusiness.Services.Interfaces;
using CrmSmallBusiness.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CrmSmallBusiness.Services;

public class BusinessService(ApplicationDbContext context) : IBusinessService
{
    public async Task<IReadOnlyCollection<BusinessListItemViewModel>> GetAllAsync(string currentUserId)
    {
        return await context.Businesses
            .AsNoTracking()
            .Include(business => business.Investments)
            .OrderByDescending(business => business.CreatedOnUtc)
            .Select(business => new BusinessListItemViewModel
            {
                Id = business.Id,
                Name = business.Name,
                OwnerName = business.OwnerName,
                Email = business.Email,
                PhoneNumber = business.PhoneNumber,
                Industry = business.Industry,
                Address = business.Address,
                Description = business.Description,
                CreatedBy = business.OwnerName,
                CreatedOnUtc = business.CreatedOnUtc,
                IsSeededExample = business.IsSeededExample,
                IsOwnedByCurrentUser = business.CreatedByUserId == currentUserId,
                TotalInvested = business.Investments.Sum(investment => investment.Amount),
                InvestorCount = business.Investments.Count,
                Investors = business.CreatedByUserId == currentUserId
                    ? business.Investments
                        .OrderByDescending(investment => investment.CreatedOnUtc)
                        .Select(investment => new BusinessInvestorViewModel
                        {
                            InvestorName = investment.InvestorName,
                            Amount = investment.Amount,
                            Note = investment.Note,
                            CreatedOnUtc = investment.CreatedOnUtc
                        })
                        .ToArray()
                    : Array.Empty<BusinessInvestorViewModel>()
            })
            .ToArrayAsync();
    }

    public async Task CreateAsync(BusinessFormViewModel model, string userId, string userDisplayName)
    {
        var business = new Business
        {
            Name = model.Name.Trim(),
            OwnerName = string.IsNullOrWhiteSpace(model.OwnerName) ? userDisplayName : model.OwnerName.Trim(),
            Email = model.Email.Trim(),
            PhoneNumber = model.PhoneNumber.Trim(),
            Industry = model.Industry.Trim(),
            Address = model.Address.Trim(),
            Description = model.Description.Trim(),
            CreatedByUserId = userId,
            CreatedOnUtc = DateTime.UtcNow
        };

        context.Businesses.Add(business);
        await context.SaveChangesAsync();
    }

    public async Task InvestAsync(
        int businessId,
        BusinessInvestmentFormViewModel model,
        string investorUserId,
        string investorDisplayName)
    {
        var businessExists = await context.Businesses.AnyAsync(business => business.Id == businessId);
        if (!businessExists)
        {
            throw new InvalidOperationException("Business not found.");
        }

        var investment = new BusinessInvestment
        {
            BusinessId = businessId,
            InvestorUserId = investorUserId,
            InvestorName = investorDisplayName,
            Amount = model.Amount,
            Note = model.Note.Trim(),
            CreatedOnUtc = DateTime.UtcNow
        };

        context.BusinessInvestments.Add(investment);
        await context.SaveChangesAsync();
    }
}
