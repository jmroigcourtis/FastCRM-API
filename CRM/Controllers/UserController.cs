using CRM.Data;
using CRM.Data.Entities;
using CRM.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using CRM.Services.Contracts;

namespace CRM.Controllers;
[ApiController]
[Route("[controller]/[action]")]
public class UserController: ControllerBase
{
    

    private readonly ILogger<UserController> _logger;
    private readonly IUserAccountService _userAccountService;
    
    public UserController (ILogger<UserController> logger, IUserAccountService userAccountService)
    {
        _userAccountService = userAccountService;
       _logger = logger;
    }
    

    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult> Login(UserLoginDto user)
    {
        if (user.Email.IsNullOrEmpty() || user.Password.IsNullOrEmpty()) return BadRequest();

       var loginToken = await _userAccountService.LoginAsync(user);
       
       _logger.LogInformation($"User logged in");
       
       return Ok(loginToken);
       
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult> GenerateTemporaryPassword(string username, string email)
    {
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email))
            return BadRequest("Invalid username or email");

        var result = await _userAccountService.GenerateNewPasswordAsync(username, email);

        return Ok(result);
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult<UsersDto>> Register (UserRegDto user)
    {
        if (user.Email.IsNullOrEmpty()) return BadRequest();
        
        var newUser = await _userAccountService.RegisterAsync(user);
    
        return Ok(newUser);
    }
    
    
    [HttpDelete]
    [AllowAnonymous]
    public async Task<ActionResult<string>> DeleteUserByUsername(string username, string email)
    {

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email)) return BadRequest(); 
        
        await _userAccountService.DeleteUserAsync(username, email);
        
        return Ok($"User {username} deleted");
    }
}