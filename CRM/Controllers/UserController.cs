using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using CRM.Data;
using CRM.Data.Entities;
using CRM.Models.Dto;
using CRM.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;
using CRM.Models;

namespace CRM.Controllers;
[ApiController]
[Route("[controller]/[action]")]
public class UserController: ControllerBase
{
    
    private readonly CrmdbContext _context;
    private readonly ILogger<UserController> _logger;
    private readonly IConfiguration _configuration;
    private DbLoggerService _dbLoggerService;
    private readonly PasswordService _passwordService;
    private readonly MailService _mailService;
    
    public UserController(CrmdbContext context, ILogger<UserController> logger, IConfiguration configuration, PasswordService passwordService, MailService mailService)
    {
       _context = context;
       _logger = logger;
       _configuration = configuration;
       _passwordService = passwordService;
       _mailService = mailService;
    }
    

    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult> Login(string username, string password )
    {
        if (username.IsNullOrEmpty() ||
            password.IsNullOrEmpty()) return BadRequest();
        

        var user = await _context.Set<UsersEntity>().FirstOrDefaultAsync(row => row.username == username);
        
        if (user == null) return BadRequest("No user found");
        
        var checkOnPassword = _passwordService.VerifyPassword(password, user.password);
        _logger.LogInformation($"User {checkOnPassword} logged in");
        
        if (checkOnPassword == false)
        {
            return BadRequest("Invalid username or password");
        }
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var key = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.username),
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
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return Ok(new
        {
            Token = tokenHandler.WriteToken(token),
        });
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult> GenerateTemporaryPassword(string username, string email)
    {
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email))
            return BadRequest("Invalid username or email");

        var locateUser = await _context.Set<UsersEntity>()
            .FirstOrDefaultAsync(row => row.username == username);
    
        if (locateUser == null) return BadRequest("No user found");

        // Generate a secure random temporary password
        var chars = Guid.NewGuid().ToString("N").Substring(0, 8) + "!";

        var hashPassword = _passwordService.HashPassword(chars);
    
        locateUser.password = hashPassword;
        _context.Update(locateUser);
        await _context.SaveChangesAsync();

        var message =
            "Hola, te enviamos tu nueva contraseña de uso temporal para que puedas crear una nueva desde el portal.\n" +
            $"Usuario: {locateUser.username} \n" +
            $"Contraseña: {chars}";
    
        _mailService.SendMail(locateUser.email, message, "CRM - Recuperación de contraseña");

        return Ok("Password reset successful");
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult<UsersDto>> Register (UsersDto user)
    {
        if (user.ToString().IsNullOrEmpty())
        {
            return BadRequest();
        }
        
        var emailAlreadyExists = await _context.Set<UsersEntity>()
            .FirstOrDefaultAsync(u => u.email == user.email);

        if (emailAlreadyExists != null)
        {
            _logger.LogError($"Email {user.email} already exists");
            return BadRequest($"Email {user.email} already exists");
        }
        
        
        var hashedPassword = _passwordService.HashPassword(user.password);

        var newUser = new UsersEntity
        {
            username = user.username,
            email = user.email,
            password = hashedPassword,
        };
        
        
        await _context.Users.AddAsync(newUser);
        await _context.SaveChangesAsync();
        _logger.LogInformation($"User created: {newUser.username}");
        var message = $"Bienvenido a CRM.\n Usuario: {user.username}.\n Ya puedes iniciar sesión.";

        _mailService.SendMail(newUser.email, message, "Bienvenido a tu CRM");

        DbLoggerService createLog = new DbLoggerService(_context);
        _dbLoggerService = createLog;
        
        var getIdUserByEmail = _context.Users.FirstOrDefault(u => u.email == user.email);

        if (getIdUserByEmail == null)
        {
            _logger.LogError($"Email {user.email} does not exist");
            return BadRequest($"Email {user.email} does not exist");
        }
        
        var newUserLog = new UserLogsDto
        {
            recordId = getIdUserByEmail.Id,
            userName = getIdUserByEmail.username,
            action = "INSERT",
            details = getIdUserByEmail.email
        };
        
        await _dbLoggerService.WriteUserLogs(newUserLog);
        
        return Ok($"User created: {newUser.username}");
    }

    [HttpPut]
    [Authorize]
    public async Task<ActionResult<UsersDto>> UpdateLoginDate(string userEmail)
    {
        if (userEmail.IsNullOrEmpty())
        {
            return BadRequest();
        }
        
        var userInDb = await _context.Set<UsersEntity>()
            .FirstOrDefaultAsync(u => u.email == userEmail);

        if (userInDb == null)
        {
            _logger.LogError($"User with email {userEmail} not found");
            return NotFound();
        }
        
        userInDb.lastLogin = DateTime.Now;
        
        await _context.SaveChangesAsync();
        
        _logger.LogInformation($"User {userInDb.username} last login date updated - {userInDb.lastLogin}");
        
        var userLoginUpdate = new UserLogsDto
        {
            recordId = userInDb.Id,
            userName = userInDb.username,
            action = "UPDATE",
            details = $"Last login - {userInDb.lastLogin.ToString()}"
        };

        await _dbLoggerService.WriteUserLogs(userLoginUpdate);
        
        return Ok($"User {userInDb.username} last login date - {userInDb.lastLogin}");
    }
    
    [HttpDelete]
    [Authorize]
    public async Task<ActionResult<UsersDto>> DeleteUserByUsername(string username)
    {

        if (username.IsNullOrEmpty())
        {
            return BadRequest();
        }
        var selectUser = await _context.Users
            .FirstOrDefaultAsync(u => u.username == username);
        
        if (selectUser == null)
        {
            _logger.LogError($"User with username {username} not found");
            return NotFound();
        }
        
        _context.Users.Remove(selectUser);
        await _context.SaveChangesAsync();
        _logger.LogInformation($"User {username} deleted");

        UserLogsDto deletedUserLog = new UserLogsDto
        {
            recordId = selectUser.Id,
            userName = selectUser.username,
            action = "DELETE",
            details = $"User {selectUser.username} deleted"
        };
        
        await _dbLoggerService.WriteUserLogs(deletedUserLog);
        
        return Ok($"User {username} deleted");
    }
}