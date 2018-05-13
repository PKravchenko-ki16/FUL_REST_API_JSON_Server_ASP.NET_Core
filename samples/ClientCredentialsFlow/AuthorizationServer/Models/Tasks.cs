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
        public int? Id { get; set; }
        [MaxLength(200)]
        [Required]
        public string Title { get; set; }
        [Required]
        public string Priority { get; set; }
        [Required]
        public DateTime Deadline { get; set; }

        public int? Id_Task { get; set; }
        [MaxLength(3)]
        [Required]
        public string Complite { get; set; }

        public int projectId { get; set; }
    }
}
