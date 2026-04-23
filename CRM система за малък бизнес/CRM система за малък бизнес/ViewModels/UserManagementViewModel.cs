namespace CrmSmallBusiness.ViewModels;

public class UserManagementViewModel
{
    public IReadOnlyCollection<UserRoleItemViewModel> Users { get; init; } = [];
    public bool CanAssignRoles { get; init; }
}
