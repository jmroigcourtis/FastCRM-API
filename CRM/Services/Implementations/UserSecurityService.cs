using CRM.Services.Contracts;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace CRM.Services.Implementations
{
    public class UserSecurityService: IUserSecurityService
    {
        private readonly ILogger<QueryDbService> _logger;
        private readonly IConfiguration _configuration;
        private readonly PasswordHasher<object> _passwordHasher;


        public UserSecurityService(ILogger<QueryDbService> logger, IConfiguration configuration, PasswordHasher<object> passwordHasher)
        {
            _logger = logger;
            _configuration = configuration;
            _passwordHasher = passwordHasher;
        }

        public string CreateBearerToken(string username)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim("role", "Admin"),
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"],
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var createToken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(createToken);
            
            return token;
        }
        
        public string HashPassword(string password)
        {
            _logger.LogInformation($"Hashing password: {password}");
            return _passwordHasher.HashPassword(null, password);
        }

        public bool VerifyPassword(string inputPassword, string storedHash)
        {
            _logger.LogInformation($"Verifying password: {inputPassword}");
            var result = _passwordHasher.VerifyHashedPassword(null, storedHash, inputPassword);
            _logger.LogInformation($"Password verification result: {result}");
            return result == PasswordVerificationResult.Success;
        }
    }
    
}

