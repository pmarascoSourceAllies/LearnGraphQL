

namespace ChelsEsite.GoldenAfternoon.Inputs
{
    public sealed record CreatePaymentInput
    {
        [GraphQLNonNullType]
        public required Guid OrderId { get; set; }  // ✅ Required Order ID

        [GraphQLNonNullType]
        public required decimal Amount { get; set; }  // ✅ Required payment amount

        [GraphQLNonNullType]
        public required string Method { get; set; } // ✅ Required payment method (e.g., Credit Card, PayPal)
    }

}