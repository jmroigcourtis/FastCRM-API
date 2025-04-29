using CRM.Models.Dto;

namespace CRM.Repositories;

public interface IDbLoggerRepository
{
    /// <summary>
    /// Writes user activity logs 
    /// </summary>
    /// <param name="logBody"></param>
    /// <returns></returns>
    Task<bool> WriteUserLogs(UserLogsDto logBody);
}