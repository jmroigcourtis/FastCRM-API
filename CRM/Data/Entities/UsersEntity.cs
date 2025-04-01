using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CRM.Data.Entities
{
    [Table("Users")]
    public class UsersEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(55)]
        [Column(TypeName = "nvarchar(55)")] // Especificar tipo de columna en SQL Server
        public string username { get; set; }

        [Required]
        [MaxLength(55)]
        [Column(TypeName = "nvarchar(55)")] // Especificar tipo de columna en SQL Server
        public string email { get; set; }

        [Required]
        [Column(TypeName = "bit")] 
        public bool isAdmin { get; set; }

        [Required]
        [Column(TypeName = "datetime")] 
        public DateTime whenCreated { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? lastLogin { get; set; }

        [Required]
        [MaxLength(255)] 
        [Column(TypeName = "nvarchar(255)")] 
        public string password { get; set; }
    }
}