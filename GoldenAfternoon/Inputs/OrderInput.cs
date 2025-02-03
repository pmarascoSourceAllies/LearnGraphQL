namespace ChelsEsite.GoldenAfternoon.Inputs
{
    public sealed record CreateOrderInput
    {
        [GraphQLNonNullType]
        public required Guid UserId { get; set; }  // ✅ Reference to the User

        [GraphQLNonNullType]
        public required ICollection<OrderItemInput> Items { get; init; }  // ✅ List of order items

        [GraphQLNonNullType]
        public required CreatePaymentInput Payment { get; init; }  // ✅ Payment details
    }

}