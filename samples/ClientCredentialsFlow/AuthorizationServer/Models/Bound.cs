using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizationServer.Models
{
    [Table("Bound")]
    public class Bound
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int? Id { get; set; }

        [Column("id_profile")]
        public int? ProfileId { get; set; }

        [Column("id_project")]
        public int? ProjectId { get; set; }
    }
}
