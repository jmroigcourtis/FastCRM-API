using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRM.Data.Entities;

[Table("TasksFollowUp")]
public class TasksFollowUpEntity
{
    [Required]
    [Key]
    public int Id { get; set; }

    [MaxLength(255)]
    [Column(TypeName = "nvarchar(255)")]
    public string TaskName { get; set; }

    public int ClientId { get; set; }

    [Required]
    [Column(TypeName = "datetime")]
    public DateTime StartDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdatedDate { get; set; } 

    [Column(TypeName = "datetime")]
    public DateTime? EndDate { get; set; }

    [MaxLength(255)]
    [Column(TypeName = "nvarchar(255)")]
    public string Priority { get; set; }

    [MaxLength(255)]
    [Column(TypeName = "nvarchar(255)")]
    public string Status { get; set; }

    [MaxLength(255)]
    [Column(TypeName = "nvarchar(255)")]
    public string AssignedTo { get; set; }
}
