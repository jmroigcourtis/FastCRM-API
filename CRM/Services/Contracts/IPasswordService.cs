namespace CRM.Services.Contracts
{
    public interface IPasswordService
    {
        /// <summary>
        /// Hash password
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        string HashPassword(string password);

        /// <summary>
        /// Verify hashed password against input
        /// </summary>
        /// <param name="inputPassword"></param>
        /// <param name="storedHash"></param>
        /// <returns></returns>
        bool VerifyPassword(string inputPassword, string storedHash);
    }
    
}

