using CrmSmallBusiness.ViewModels;

namespace CrmSmallBusiness.Services.Interfaces;

public interface IBusinessService
{
    Task<IReadOnlyCollection<BusinessListItemViewModel>> GetCatalogAsync(string currentUserId);
    Task<IReadOnlyCollection<OwnedBusinessItemViewModel>> GetOwnedAsync(string currentUserId);
    Task<IReadOnlyCollection<OwnedBusinessOptionViewModel>> GetOwnedOptionsAsync(string currentUserId);
    Task<bool> HasOwnedBusinessAsync(string currentUserId);
    Task CreateAsync(BusinessFormViewModel model, string userId, string userDisplayName);
    Task UpdateAsync(int businessId, BusinessFormViewModel model, string currentUserId);
    Task InvestAsync(int businessId, BusinessInvestmentFormViewModel model, string investorUserId, string investorDisplayName);
}
