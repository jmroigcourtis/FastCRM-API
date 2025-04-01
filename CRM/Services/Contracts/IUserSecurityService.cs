namespace CRM.Services.Contracts
{
    public interface IUserSecurityService
    {
        string CreateBearerToken(string username);

        string HashPassword(string password);

        bool VerifyPassword(string inputPassword, string storedHash);
        
        
    }
}

