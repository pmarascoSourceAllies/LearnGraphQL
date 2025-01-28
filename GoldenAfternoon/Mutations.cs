using ChelsEsite.GoldenAfternoon.Data;

namespace ChelsEsite.GoldenAfternoon;

public static class Mutations
{
    [Mutation]
    public static async Task<AddUserPayload> AddUserAsync(
        AddUserInput input,
        ApplicationDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var user = new User
        {
            Name = input.Name,
            Email = input.Email,
            Role = input.Role,
            PasswordHash = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(input.Password)),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        dbContext.Users.Add(user);

        await dbContext.SaveChangesAsync(cancellationToken);

        return new AddUserPayload(user);
    }
}