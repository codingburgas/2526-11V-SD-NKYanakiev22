using CrmSmallBusiness.Data;
using CrmSmallBusiness.Domain.Entities;
using CrmSmallBusiness.Services.Interfaces;
using CrmSmallBusiness.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CrmSmallBusiness.Services;

public class BusinessService(ApplicationDbContext context) : IBusinessService
{
    public async Task<IReadOnlyCollection<BusinessListItemViewModel>> GetCatalogAsync(string currentUserId)
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
                CanInvest = business.CreatedByUserId != currentUserId,
                TotalInvested = business.Investments.Sum(investment => investment.Amount),
                InvestorCount = business.Investments.Count
            })
            .ToArrayAsync();
    }

    public async Task<IReadOnlyCollection<OwnedBusinessItemViewModel>> GetOwnedAsync(string currentUserId)
    {
        return await context.Businesses
            .AsNoTracking()
            .Where(business => business.CreatedByUserId == currentUserId)
            .Include(business => business.Investments)
            .OrderByDescending(business => business.CreatedOnUtc)
            .Select(business => new OwnedBusinessItemViewModel
            {
                Id = business.Id,
                IsSeededExample = business.IsSeededExample,
                TotalInvested = business.Investments.Sum(investment => investment.Amount),
                InvestorCount = business.Investments.Count,
                Form = new BusinessFormViewModel
                {
                    Name = business.Name,
                    OwnerName = business.OwnerName,
                    Email = business.Email,
                    PhoneNumber = business.PhoneNumber,
                    Industry = business.Industry,
                    Address = business.Address,
                    Description = business.Description
                },
                Investors = business.Investments
                    .OrderByDescending(investment => investment.CreatedOnUtc)
                    .Select(investment => new BusinessInvestorViewModel
                    {
                        InvestorName = investment.InvestorName,
                        Amount = investment.Amount,
                        Note = investment.Note,
                        CreatedOnUtc = investment.CreatedOnUtc
                    })
                    .ToArray()
            })
            .ToArrayAsync();
    }

    public async Task<IReadOnlyCollection<OwnedBusinessOptionViewModel>> GetOwnedOptionsAsync(string currentUserId)
    {
        return await context.Businesses
            .AsNoTracking()
            .Where(business => business.CreatedByUserId == currentUserId)
            .OrderBy(business => business.Name)
            .Select(business => new OwnedBusinessOptionViewModel
            {
                Id = business.Id,
                Name = business.Name
            })
            .ToArrayAsync();
    }

    public async Task<bool> HasOwnedBusinessAsync(string currentUserId)
    {
        return await context.Businesses.AnyAsync(business => business.CreatedByUserId == currentUserId);
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

    public async Task UpdateAsync(int businessId, BusinessFormViewModel model, string currentUserId)
    {
        var business = await context.Businesses
            .FirstOrDefaultAsync(item => item.Id == businessId && item.CreatedByUserId == currentUserId);

        if (business is null)
        {
            throw new InvalidOperationException("Business not found.");
        }

        business.Name = model.Name.Trim();
        business.OwnerName = model.OwnerName.Trim();
        business.Email = model.Email.Trim();
        business.PhoneNumber = model.PhoneNumber.Trim();
        business.Industry = model.Industry.Trim();
        business.Address = model.Address.Trim();
        business.Description = model.Description.Trim();
        business.ModifiedOnUtc = DateTime.UtcNow;

        await context.SaveChangesAsync();
    }

    public async Task InvestAsync(
        int businessId,
        BusinessInvestmentFormViewModel model,
        string investorUserId,
        string investorDisplayName)
    {
        var business = await context.Businesses
            .FirstOrDefaultAsync(item => item.Id == businessId);

        if (business is null)
        {
            throw new InvalidOperationException("Business not found.");
        }

        if (business.CreatedByUserId == investorUserId)
        {
            throw new InvalidOperationException("You cannot invest in your own business.");
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
