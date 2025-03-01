using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CRM.Data.Entities;
[Table("Users")]
public class UsersEntity
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(55)]
    public string username { get; set; }
    
    [Required]
    [MaxLength(55)]
    public string email { get; set; }
    
    [Required]
    [MaxLength(55)]
    
    public bool isAdmin { get; set; }
    
    [Required]
    public DateTime whenCreated { get; set; }
    
    public DateTime? lastLogin { get; set; }
    
    [Required]
    public string password { get; set; }
}