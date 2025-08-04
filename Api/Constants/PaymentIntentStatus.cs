namespace Api.Constants
{
    // for more about paymentIntent status
    // https://docs.stripe.com/payments/paymentintents/lifecycle?payment-setup-intent=paymentintent
    public enum PaymentIntentStatus
    {
        Unknown,
        RequiresPaymentMethod,
        RequiresConfirmation,
        RequiresAction,
        Processing,
        Succeeded,
        Canceled
    }

    public static class PaymentIntentStatusExtensions
    {
        public static PaymentIntentStatus FromStripeString(string status) => status switch
        {
            "requires_payment_method" => PaymentIntentStatus.RequiresPaymentMethod,
            "requires_confirmation" => PaymentIntentStatus.RequiresConfirmation,
            "requires_action" => PaymentIntentStatus.RequiresAction,
            "processing" => PaymentIntentStatus.Processing,
            "succeeded" => PaymentIntentStatus.Succeeded,
            "canceled" => PaymentIntentStatus.Canceled,
            _ => PaymentIntentStatus.Unknown,
        };

        public static string ToStripeString(this PaymentIntentStatus status) => status switch
        {
            PaymentIntentStatus.RequiresPaymentMethod => "requires_payment_method",
            PaymentIntentStatus.RequiresConfirmation => "requires_confirmation",
            PaymentIntentStatus.RequiresAction => "requires_action",
            PaymentIntentStatus.Processing => "processing",
            PaymentIntentStatus.Succeeded => "succeeded",
            PaymentIntentStatus.Canceled => "canceled",
            _ => "unknown"
        };
    }
}
