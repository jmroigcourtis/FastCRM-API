using CRM.Services.Contracts;
using Microsoft.AspNetCore.Identity;

namespace CRM.Services.Implementations
{
    public class PasswordService:IPasswordService
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
}

