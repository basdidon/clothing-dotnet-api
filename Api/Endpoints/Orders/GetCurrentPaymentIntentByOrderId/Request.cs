namespace Api.Endpoints.Orders.GetValidPaymentIntentByOrderId
{
    public class Request
    {
        public Guid OrderId { get; set; }
    }
}
