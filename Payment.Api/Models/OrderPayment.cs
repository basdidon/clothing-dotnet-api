namespace Payment.Api.Models
{
    public enum OrderPaymentStatus
    {
        Pending,
        Paid,
        Failed,
        Refunded
    }

    public class OrderPayment
    {
        public Guid Id { get; set; }

        public Guid OrderId { get; set; } // Reference to Order from Order Service
        
        public string Provider { get; set; } = "Stripe"; // Default to Stripe, can be extended for other providers
        public string PaymentIntentId { get; set; } = string.Empty;
        public string? ClientSecret { get; set; } // Optional, Client secret for Stripe payment intent     

        public OrderPaymentStatus Status { get; set; } = OrderPaymentStatus.Pending;

        public DateTime CreatedAt { get; set; }
        public DateTime? PaidAt { get; set; }
    }
}