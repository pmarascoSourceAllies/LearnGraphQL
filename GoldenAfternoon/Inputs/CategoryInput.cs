namespace ChelsEsite.GoldenAfternoon.Inputs;
public class CreateCategoryInput
{
    [GraphQLNonNullType]
    public string Name { get; init; } = string.Empty;

    public string? Description { get; set; }
}
public class UpdateCategoryInput
{
    public string? Description { get; set; }
    public ICollection<Guid>? ProductIDs { get; set; }
}
public class AddProductToCategoryInput
{
    public required ICollection<Guid> ProductIDs { get; set; }
}
