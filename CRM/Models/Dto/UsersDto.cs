namespace CRM.Models.Dto;

public class UsersDto
{
    public int Id { get; set; }
    public string username { get; set; }
    public string email { get; set; }
    public bool isAdmin { get; set; }
    public DateTime whenCreated { get; set; }
    public DateTime? lastLogin { get; set; }
    public string password { get; set; }
}