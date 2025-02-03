namespace ChelsEsite.GoldenAfternoon.Inputs;
public class CreateCategoryInput
{
    [GraphQLNonNullType]
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }
}
