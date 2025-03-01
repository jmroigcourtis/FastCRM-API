using AutoMapper;
using CRM.Data;
using CRM.Data.Entities;
using CRM.Models.Dto;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRM.Services;

public class DbLoggerService:ControllerBase
{
    private readonly CrmdbContext _context;
    private readonly ILogger<DbLoggerService> _logger;
    
    public DbLoggerService (CrmdbContext context)
    {
        _context = context;
    }

    public async Task<ActionResult<string>> WriteUserLogs(UserLogsDto logBody)
    {
        if (logBody == null)
        {
            _logger.LogError("logBody is null");
            return BadRequest("logBody is null");
        }
        

        var newUserLogs = new UserLogsEntity
        {
            recordId = logBody.Id,
            userName = logBody.userName,
            action = logBody.action,
            details = logBody.details
        };

        await _context.UserLogs.AddAsync(newUserLogs);
        await _context.SaveChangesAsync();

        return Ok($"Log successfully written for user {logBody.userName}");
    }


}