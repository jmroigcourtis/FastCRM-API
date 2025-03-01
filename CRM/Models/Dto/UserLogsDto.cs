namespace CRM.Models.Dto;

public class UserLogsDto
{
    public int Id { get; set; }
    public int recordId { get; set; }
    public string userName { get; set; }
    
    public string action { get; set; }
    
    public string details { get; set; }
}