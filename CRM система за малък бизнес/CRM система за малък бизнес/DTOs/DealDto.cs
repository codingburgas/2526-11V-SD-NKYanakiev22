using CrmSmallBusiness.Domain.Enums;

namespace CrmSmallBusiness.DTOs;

public class DealDto
{
    public int Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string CustomerName { get; init; } = string.Empty;
    public DealStage Stage { get; init; }
    public decimal Value { get; init; }
    public DateTime ExpectedCloseDate { get; init; }
}
