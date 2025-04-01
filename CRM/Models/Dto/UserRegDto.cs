using System.ComponentModel.DataAnnotations;

namespace CRM.Models.Dto;

public class UserRegDto
{
    [Required]
    public string Username { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}