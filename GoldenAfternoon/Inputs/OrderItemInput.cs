namespace ChelsEsite.GoldenAfternoon.Inputs
{
    public class OrderItemInput
    {
        [GraphQLNonNullType]
        public required Guid ProductId { get; set; }  // ✅ Product being ordered

        [GraphQLNonNullType]
        public required int Quantity { get; set; }  // ✅ Quantity of the product
    }

}