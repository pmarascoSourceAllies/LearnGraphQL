namespace ChelsEsite.GoldenAfternoon.Data;

public sealed record AddUserInput(
    string Name,
    string Email,
    string Role,
    string Password);