

using ChelsEsite.GoldenAfternoon.Data;

namespace ChelsEsite.GoldenAfternoon.Inputs
{
    public sealed record CreateProductInput
    {
        [GraphQLNonNullType]
        public required string Name { get; set; }

        public string? Description { get; set; }

        [GraphQLNonNullType]
        public decimal Price { get; set; }

        [GraphQLNonNullType]
        public int Stock { get; set; }

        [GraphQLNonNullType]
        public required string ImageUrl { get; set; }  // Enum-based role

        public required Guid CategoryID { get; set; }
    }

    public class UpdateProductInput
    {
        public string? Name { get; set; }

        public string? Description { get; set; }

        public decimal? Price { get; set; }

        public int? Stock { get; set; }

        public string? ImageUrl { get; set; }  // Enum-based role

        public Guid? CategoryID { get; set; }
    }

}