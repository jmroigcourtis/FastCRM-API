using CRM.Models.Dto;

namespace CRM.Services.Contracts;

public interface IDbLoggerService
{
    /// <summary>
    /// Writes user activity logs 
    /// </summary>
    /// <param name="logBody"></param>
    /// <returns></returns>
    Task<bool> WriteUserLogs(UserLogsDto logBody);
}