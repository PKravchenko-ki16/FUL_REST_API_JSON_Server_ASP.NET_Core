using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthorizationServer.Models
{
    [Table("Profile")]
    public class Profile
    {
        [Required]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [MaxLength(20)]
        [Required]
        [Column("Name")]
        public string Name { get; set; }

        [MaxLength(20)]
        [Required]
        [Column("Login")]
        public string Login { get; set; }

        [MaxLength(20)]
        [Required]
        [Column("Password")]
        public string Password { get; set; }
    }
}
