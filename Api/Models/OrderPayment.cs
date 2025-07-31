using Api.Constants;

namespace Api.Models
{
    public class OrderPayment
    {
        public Guid Id { get; set; }

        public Guid OrderId { get; set; } // Reference to Order from Order Service
        public Order Order { get; set; } = null!;

        public OrderPaymentStatus Status { get; set; } = OrderPaymentStatus.Pending;

        public DateTime CreatedAt { get; set; }
        public DateTime? PaidAt { get; set; }
    }

    public class StripeOrderPayment : OrderPayment
    {
        public string PaymentIntentId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;   
    }

    public class CashOrderPayment : OrderPayment 
    { 
       
    }
}
