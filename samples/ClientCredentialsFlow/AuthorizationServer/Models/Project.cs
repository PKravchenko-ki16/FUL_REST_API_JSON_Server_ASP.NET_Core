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
        [Column("Id")]
        public int? Id { get; set; }

        [MaxLength(250)]
        [Required]
        [Column("Title")]
        public string Title { get; set; }

        [MaxLength(2000)]
        [Required]
        [Column("Description")]
        public string Description { get; set; }

        [MaxLength(10)]
        [Required]
        [Column("Arhive")]
        public string Arсhive { get; set; }

        [Required]
        [Column("Id_Profile")]
        public int? ProfileId { get; set; }
    }
}
