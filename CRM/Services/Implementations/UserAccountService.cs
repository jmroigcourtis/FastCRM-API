using AutoMapper;
using CRM.Data;
using CRM.Data.Entities;
using CRM.Models;
using CRM.Models.Dto;
using CRM.Services.Contracts;
using Microsoft.IdentityModel.Tokens;

namespace CRM.Services.Implementations
{
    public class UserAccountService: IUserAccountService
    {
        private readonly IQueryDbService _queryDbService;
        private readonly IUserSecurityService _userSecurityService;
        private readonly ILogger _logger;

        public UserAccountService(IQueryDbService queryDbService, IUserSecurityService userSecurityService, ILogger logger)
        {
            _queryDbService = queryDbService;
            _userSecurityService = userSecurityService;
            _logger = logger;
        }

        public async Task<string> LoginAsync (UserLoginDto user)
        {
            if (user.Email.IsNullOrEmpty() || user.Password.IsNullOrEmpty()) return null;

            var userExists = await _queryDbService.CheckIfEmailExists(user.Email);

            if (!userExists) return "Email doesn't exist";
        
            var userRecord = await _queryDbService.GetUserByEmail(user.Email);
        
            var checkOnPassword = _userSecurityService.VerifyPassword(user.Password, userRecord.HashedPassword);
        
            if (checkOnPassword == false) return "Invalid username or password";

            var token = _userSecurityService.CreateBearerToken(userRecord.Username);

            await _queryDbService.UpdateLoginDate(userRecord.Username, user.Email);

            return token;
        }

        public async Task<string> RegisterAsync(UserRegDto userx)
        {
            var checkUser = await _queryDbService.CheckIfEmailExists(userx.Email);

            if (checkUser) 
            {
                _logger.LogWarning("Email already exists");
                return "Email already exists";
            }

            var hashedPassword = _userSecurityService.HashPassword(userx.Password);

            var userToInsert = new UserRegDto
            {
                Email = userx.Email,
                Password = hashedPassword,
                Username = userx.Username
            };

            var newUser = await _queryDbService.InsertUser(userToInsert, hashedPassword);

            if (newUser == null)
            {
                _logger.LogError("User creation failed.");
                return "User creation failed.";
            }

            _logger.LogInformation($"User created: {newUser.username}");

            return $"User {newUser.username} created";
        }


        public async Task<string> GenerateNewPasswordAsync(string username, string email)
        {
            var userExists = await _queryDbService.CheckIfEmailExists(email);
            if (!userExists) return "Email doesn't exist";
            
            var newPw = Guid.NewGuid().ToString("N").Substring(0, 8) + "!";

            var hashedPassword = _userSecurityService.HashPassword(newPw);
            
            var result = await _queryDbService.UpdatePasswordOnDb(username, email, hashedPassword);
            
            return result;
        }

        public async Task<bool> DeleteUserAsync(string username, string email)
        {
            _logger.LogInformation($"Searching for user: {username} with email {email}");
            var deleteUser = await _queryDbService.RemoveUserOnDb(username, email);
            if (!deleteUser) return false;
            _logger.LogInformation($"User {username} deleted");
            return true;
        }

        public async Task<UsersListDto> GetAllUsers()
        {
           var users = await _queryDbService.GetUsersFromDb();

           return users;
        }
    }
    
}

