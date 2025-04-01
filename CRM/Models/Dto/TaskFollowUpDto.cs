namespace CRM.Models.Dto;

public class TaskFollowUpDto
{
    public int Id { get; set; }
    public string TaskName { get; set; }
    public int ClientId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string Priority { get; set; }
    public string Status { get; set; }
    public string AssignedTo { get; set; }
}