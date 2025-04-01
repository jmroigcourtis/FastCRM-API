using CRM.Data;
using CRM.Data.Entities;
using CRM.Models;
using CRM.Models.Dto;
using CRM.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CRM.Services.Implementations
{
    
    public class QueryDbService:IQueryDbService
    {
        private readonly ILogger<QueryDbService> _logger;
        private readonly CrmdbContext _dbContext;

        public QueryDbService(ILogger<QueryDbService> logger, CrmdbContext context)
        {
            _logger = logger;
            _dbContext = context;
        }

        public async Task<bool> CheckIfEmailExists(string email)
        {
            var user = await _dbContext.Set<UsersEntity>()
                .Where(u => u.email == email)
                .FirstOrDefaultAsync();

            if (user == null) return false; // ✅ Evita el NullReferenceException

            _logger.LogInformation($"There's an account already existing with email {user.email}.");
            return true;
        }


        public async Task<UserLogin> GetUserByEmail(string email)
        {
            var user = await _dbContext.Set<UsersEntity>().FirstOrDefaultAsync(u => u.email == email);

            if (user.username.IsNullOrEmpty())
            {
                _logger.LogInformation($"User {email} not found");
                return null;
            }

            var userFromDb = new UserLogin
            {
               Username = user.username,
               HashedPassword = user.password
            };
            
            return userFromDb;
        }

        public async Task <string> UpdateLoginDate(string username, string email)
        {
            var updateLastLoginDate = await _dbContext
                .Set<UsersEntity>()
                .FirstOrDefaultAsync(row => 
                    row.username == username 
                    && row.email == email);

            if (updateLastLoginDate == null)
            {
                _logger.LogInformation($"User {username} not found");
                return null;
            }
            
            updateLastLoginDate.lastLogin = DateTime.Now;
            
            _dbContext.Set<UsersEntity>().Update(updateLastLoginDate);
            await _dbContext.SaveChangesAsync();
            return "User last login date updated";
        }

        public async Task<UsersDto> InsertUser(UserRegDto user, string hashedPassword)
        {
            var newUser = new UsersEntity
            {
                username = user.Username,
                email = user.Email,
                password = hashedPassword,
                whenCreated = DateTime.Now,
                isAdmin = false
            };
            
            await _dbContext.Set<UsersEntity>().AddAsync(newUser);
        
            await _dbContext.SaveChangesAsync();

            return new UsersDto
            {
                username = newUser.username,
                email = newUser.email,
                whenCreated = newUser.whenCreated,
                isAdmin = newUser.isAdmin
            };
        }

        public async Task<string> UpdatePasswordOnDb(string username, string email, string pw)
        {
            var locateUser = await _dbContext.Set<UsersEntity>()
                .FirstOrDefaultAsync(row => row.username == username && row.email == email);
            
            if (locateUser == null) return null;
            
            locateUser.password = pw;
            
            _dbContext.Set<UsersEntity>().Update(locateUser);
            await _dbContext.SaveChangesAsync();
            
            return "Password updated";
        }

        public async Task<bool> RemoveUserOnDb(string username, string email)
        {
            var user = await _dbContext.Set<UsersEntity>().FirstOrDefaultAsync(u => u.username == username && u.email == email);
            if (user == null)
            {
                _logger.LogInformation($"User {username} not found");
                return false;
            }
            _dbContext.Set<UsersEntity>().Remove(user);
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation($"User {username} has been deleted");
            return true;
        }
    }
}

