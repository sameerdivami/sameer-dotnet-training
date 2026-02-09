using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace PolicyManagement.Entities
{
    [Table("User")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }
        [Column("Name")]
        public required string Name { get; set; }
        [Column("Email")]
        public required string Email { get; set; }
        [Column("PasswordHash")]
        public required string PasswordHash { get; set; }

        [Column("Role")]
        public required string Role { get; set; }
    }
}