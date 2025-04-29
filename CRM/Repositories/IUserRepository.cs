using CRM.Models;
using CRM.Models.Dto;

namespace CRM.Repositories;

public interface IUserRepository
{
    /// <summary>
            /// Search user on UsersTable by using the username or email
            /// </summary>
            /// <param name="email"></param>
            /// <returns></returns>
            Task<bool> CheckIfEmailExists (string email);
            
            /// <summary>
            ///  Get User data from UserTable by using email
            /// </summary>
            /// <param name="email"></param>
            /// <returns></returns>
            Task<UserLogin> GetUserByEmail(string email);
    
            /// <summary>
            /// Updates last login date on DB
            /// </summary>
            /// <param name="username"></param>
            /// <param name="email"></param>
            /// <returns></returns>
            Task<string> UpdateLoginDate(string username, string email);
    
            /// <summary>
            ///  Insert new user in DB
            /// </summary>
            /// <param name="user"></param>
            /// <param name="hashedPassword"></param>
            /// <returns></returns>
            Task<UsersDto> InsertUser(UserRegDto user, string hashedPassword);
    
            /// <summary>
            /// Update password row in DB
            /// </summary>
            /// <param name="username"></param>
            /// <param name="email"></param>
            /// <param name="pw"></param>
            /// <returns></returns>
            Task<string> UpdatePasswordOnDb(string username, string email, string pw);
    
            /// <summary>
            /// Removes user from database
            /// </summary>
            /// <param name="username"></param>
            /// <param name="email"></param>
            /// <returns></returns>
            Task<bool> RemoveUserOnDb(string username, string email);
    
            Task<UsersListDto> GetUsersFromDb();
}