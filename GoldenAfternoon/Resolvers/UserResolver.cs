using ChelsEsite.GoldenAfternoon.Data;
using ChelsEsite.GoldenAfternoon.Inputs;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

public class UserResolver
{
    private readonly ApplicationDbContext _dbContext;
    private PasswordHasher<string> passwordHasher = new PasswordHasher<string>();

    public UserResolver(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    [Query]
    public async Task<IEnumerable<User>> GetUsers(string? search, Role? role, CancellationToken cancellationToken)
    {
        var query = _dbContext.Users.AsQueryable();

        if (!string.IsNullOrEmpty(search))
            query = query.Where(u => u.Name.Contains(search));

        if (role != null)
            query = query.Where(u => u.Role == role);

        return await query.ToListAsync(cancellationToken);
    }

    [Query]
    public async Task<User?> GetUser(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.Users.FindAsync(id, cancellationToken);
    }

    [Mutation]
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

    [Mutation]
    public async Task<User> UpdateUser(Guid id, UpdateUserInput input, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.FindAsync(id);
        if (user == null) throw new Exception("User not found.");

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
        var user = await _dbContext.Users.FindAsync(id);
        if (user == null) return false;

        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}
