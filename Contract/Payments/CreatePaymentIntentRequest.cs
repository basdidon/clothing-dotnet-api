namespace Contract.Payments
{
    public record CreatePaymentIntentRequest(Guid OrderId, decimal TotalAmount);
    public record CreatePaymentIntentResponse(string ClientSecret);
}
