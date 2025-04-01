using System.ComponentModel.DataAnnotations;

namespace CRM.Models;

public class UserLogin
{
    public string Username { get; set; }
    public string HashedPassword { get; set; }
}