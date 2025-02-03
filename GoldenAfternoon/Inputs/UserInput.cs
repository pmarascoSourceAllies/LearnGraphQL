

using ChelsEsite.GoldenAfternoon.Data;

namespace ChelsEsite.GoldenAfternoon.Inputs;

public sealed record CreateUserInput
{
    [GraphQLNonNullType]
    public string Name { get; set; } = string.Empty;

    [GraphQLNonNullType]
    public string Email { get; set; } = string.Empty;

    [GraphQLNonNullType]
    public string Password { get; set; } = string.Empty;

    [GraphQLNonNullType]
    public Role Role { get; set; }  // Enum-based role
}

public sealed record UpdateUserInput
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public Role? Role { get; set; }  // Enum-based role
}