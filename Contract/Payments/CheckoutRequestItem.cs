namespace Contract.Payments
{
    public record CheckoutRequestItem
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}