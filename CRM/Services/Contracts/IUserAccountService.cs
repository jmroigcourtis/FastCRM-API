using CRM.Models.Dto;

namespace CRM.Services.Contracts;

public interface IUserAccountService
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    Task<string> LoginAsync (UserLoginDto user);

    /// <summary>
    ///
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    Task<string> RegisterAsync(UserRegDto user);

    /// <summary>
    /// Create new password
    /// </summary>
    /// <param name="username"></param>
    /// <param name="email"></param>
    /// <returns></returns>
    Task<string> GenerateNewPasswordAsync(string username, string email);
    
    /// <summary>
    /// Delete user
    /// </summary>
    /// <param name="username"></param>
    /// <param name="email"></param>
    /// <returns></returns>
    Task<bool> DeleteUserAsync(string username, string email);
}