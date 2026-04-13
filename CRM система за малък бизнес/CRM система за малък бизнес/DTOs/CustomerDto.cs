namespace CrmSmallBusiness.DTOs;

public class CustomerDto
{
    public int Id { get; init; }
    public string FullName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string CompanyName { get; init; } = string.Empty;
    public string Position { get; init; } = string.Empty;
}
