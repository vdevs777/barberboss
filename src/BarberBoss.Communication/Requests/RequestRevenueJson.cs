using BarberBoss.Communication.Enums;

namespace BarberBoss.Communication.Requests;

public class RequestRevenueJson
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime OccurredAt { get; set; }
    public decimal Amount { get; set; }
    public PaymentType PaymentType { get; set; }
}

