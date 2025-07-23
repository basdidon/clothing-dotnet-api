namespace Contract.Payments
{
    public record CheckoutRequest(Guid OrderId,Dictionary<Guid,CheckoutRequestItem> CheckoutItems);
    public record CheckoutResponse(string ClientSecret);
}