using Microsoft.AspNetCore.Identity;

namespace CRM.Services;

public class PasswordService
{
    private readonly PasswordHasher<object> _passwordHasher = new PasswordHasher<object>();

    public string HashPassword(string password)
    {
        return _passwordHasher.HashPassword(null, password);
    }

    public bool VerifyPassword(string inputPassword, string storedHash)
    {
        var result = _passwordHasher.VerifyHashedPassword(null, storedHash, inputPassword);
        return result == PasswordVerificationResult.Success;
    }
}
