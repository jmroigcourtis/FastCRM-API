using CRM.Data.Entities;
using Microsoft.EntityFrameworkCore;


namespace CRM.Data;

public class CrmdbContext : DbContext
{
    
    private readonly string _connectionString;

    public CrmdbContext(IConfiguration configuration, DbContextOptions<CrmdbContext> options) : base(options)
    {
        
        _connectionString = configuration.GetConnectionString("DB");
        
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySql(_connectionString, ServerVersion.AutoDetect(_connectionString));
    }
    public DbSet <UsersEntity> Users { get; set; }
    
    public DbSet <UserLogsEntity> UserLogs { get; set; }


}