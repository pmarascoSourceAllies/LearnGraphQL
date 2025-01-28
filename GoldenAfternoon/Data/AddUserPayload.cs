namespace ChelsEsite.GoldenAfternoon.Data
{
    public sealed class AddUserPayload(User user)
    {
        public User User { get; } = user;
    }
}