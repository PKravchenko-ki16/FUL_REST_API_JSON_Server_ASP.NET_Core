using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizationServer.Models
{
    [Table("Task")]
    public class Tasks
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        [Column("Id")]
        public int? Id { get; set; }
        [MaxLength(200)]
        [Required]
        [Column("Title")]
        public string Title { get; set; }
        [Required]
        [Column("Priority")]
        public string Priority { get; set; }
        [Required]
        [Column("Deadline")]
        public DateTime Deadline { get; set; }

        [Column("Id_Task")]
        public int? TaskId { get; set; }
        [MaxLength(3)]
        [Required]
        [Column("Complite")]
        public string Complite { get; set; }

        [Column("projectId")]
        [Required]
        public int ProjectId { get; set; }
    }
}
