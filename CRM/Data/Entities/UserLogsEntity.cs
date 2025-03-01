using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRM.Data.Entities;

[Table("UserLogs")]
public class UserLogsEntity
{
    [Key]
    public int Id { get; set; }
    
    public int recordId { get; set; }

    public string userName { get; set; }
    
    public string action { get; set; }
    
    public string details { get; set; }

}