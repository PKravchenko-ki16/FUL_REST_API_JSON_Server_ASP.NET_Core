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
        public int Id { get; set; }

        [MaxLength(20)]
        [Required]
        public string Name { get; set; }

        [MaxLength(20)]
        [Required]
        public string Login { get; set; }

        [MaxLength(20)]
        [Required]
        public string Password { get; set; }
    }
}
