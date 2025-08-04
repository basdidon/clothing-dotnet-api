using Api.Constants;
using Api.Models;

namespace Api.DTOs
{
    public class OrderPaymentIntentDto
    {
        public string PaymentIntentId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;

        public Guid OrderId { get; set; }

        public PaymentIntentStatus Status { get; set; } = PaymentIntentStatus.Unknown;

        public DateTime CreatedAt { get; set; }

        public static OrderPaymentIntentDto Map(OrderPaymentIntent orderPaymentIntent) => new()
        {
            PaymentIntentId = orderPaymentIntent.PaymentIntentId,
            ClientSecret = orderPaymentIntent.ClientSecret,
            OrderId = orderPaymentIntent.OrderId,
            Status = orderPaymentIntent.Status,
            CreatedAt = orderPaymentIntent.CreatedAt,
        };
    }
}
