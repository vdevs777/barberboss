using BarberBoss.Communication.Enums;

namespace BarberBoss.Communication.Responses;

public class ResponseRevenueJson
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime OccurredAt { get; set; }
    public decimal Amount { get; set; }
    public PaymentType PaymentType { get; set; }
}



