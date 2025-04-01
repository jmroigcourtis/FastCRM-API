using CRM.Data;
using CRM.Data.Entities;
using CRM.Models.Dto;
using CRM.Services.Contracts;


namespace CRM.Services.Implementations
{
    public class DbLoggerService: IDbLoggerService
    {
        private readonly CrmdbContext _context;
        private readonly ILogger<DbLoggerService> _logger;
    
        public DbLoggerService (CrmdbContext context, ILogger<DbLoggerService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> WriteUserLogs(UserLogsDto logBody)
        {
            var newUserLogs = new UserLogsEntity
            {
                recordId = logBody.Id,
                userName = logBody.userName,
                action = logBody.action,
                details = logBody.details
            };

            await _context.UserLogs.AddAsync(newUserLogs);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"UserLogs added: {newUserLogs.recordId}");
            return true;
        }
    }
}

