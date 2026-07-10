namespace server.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(string userId, string email, string Name, string role);
    }
}
