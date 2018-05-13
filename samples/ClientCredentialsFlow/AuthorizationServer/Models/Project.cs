using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizationServer.Models
{
    [Table("Project")]
    public class Project
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int? Id { get; set; }

        [MaxLength(250)]
        [Required]
        public string Title { get; set; }

        [MaxLength(2000)]
        [Required]
        public string Description { get; set; }

        [MaxLength(10)]
        [Required]
        public string Arhive { get; set; }

        [Required]
        public int? Id_Profile { get; set; }
    }
}
