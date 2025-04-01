using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRM.Data.Entities
{
    [Table("UserLogs")]
    public class UserLogsEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int recordId { get; set; }

        [MaxLength(255)]
        [Column(TypeName = "nvarchar(255)")]
        public string userName { get; set; }

        [MaxLength(255)]
        [Column(TypeName = "nvarchar(255)")] 
        public string action { get; set; }

        [MaxLength(1000)] 
        [Column(TypeName = "nvarchar(max)")] 
        public string details { get; set; }
    }
}