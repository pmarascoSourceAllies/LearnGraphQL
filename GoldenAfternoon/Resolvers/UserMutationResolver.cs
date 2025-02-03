using ChelsEsite.GoldenAfternoon.Data;
using ChelsEsite.GoldenAfternoon.Inputs;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ChelsEsite.GoldenAfternoon.Resolvers;
[MutationType]
public class UserMutatonResolver
{
    private readonly ApplicationDbContext _dbContext;
    private PasswordHasher<string> passwordHasher = new PasswordHasher<string>();

    public UserMutatonResolver(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<User> CreateUser(CreateUserInput input, CancellationToken cancellationToken)
    {
        if (await _dbContext.Users.AnyAsync(u => u.Email == input.Email))
            throw new Exception("Email already exists.");


        var hashedPassword = passwordHasher.HashPassword(input.Email, input.Password);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = input.Name,
            Email = input.Email,
            Role = input.Role,
            PasswordHash = hashedPassword
        };

        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return user;
    }
    public async Task<User> UpdateUser(Guid id, UpdateUserInput input, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.FindAsync([id], cancellationToken: cancellationToken) ?? throw new Exception("User not found.");
        user.Name = input.Name ?? user.Name;
        if (input.Email != null)
        {
            if (await _dbContext.Users.AnyAsync(u => u.Email == input.Email))
                throw new Exception("Email already exists.");
            user.Email = input.Email ?? user.Email;
        }
        if (input.Password != null)
        {
            user.PasswordHash = passwordHasher.HashPassword(user.Email, input.Password);
        }
        user.Role = input.Role ?? user.Role;

        await _dbContext.SaveChangesAsync(cancellationToken);
        return user;
    }

    [Mutation]
    public async Task<bool> DeleteUser(Guid id, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.FindAsync([id], cancellationToken);
        if (user == null) return false;

        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}