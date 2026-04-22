using CrmSmallBusiness.ViewModels;

namespace CrmSmallBusiness.Services.Interfaces;

public interface IBusinessService
{
    Task<IReadOnlyCollection<BusinessListItemViewModel>> GetAllAsync(string currentUserId);
    Task CreateAsync(BusinessFormViewModel model, string userId, string userDisplayName);
    Task InvestAsync(int businessId, BusinessInvestmentFormViewModel model, string investorUserId, string investorDisplayName);
}
