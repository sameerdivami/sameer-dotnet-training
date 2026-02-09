using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace PolicyManagement.Entities
{
    [Table("UserPolicy")]
    public class UserPolicy
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [ForeignKey("User")]
        [Column("UserId")]
        public int UserId { get; set; }
        public User? User { get; set; }

        [ForeignKey("Policy")]
        [Column("PolicyId")]
        public int PolicyId { get; set; }
        public Policy? Policy { get; set; }
        [Column("Status")]
        public string Status { get; set; } = "Pending";

        [Column("RequestedAt")]
        public DateTime RequestedAt { get; set; }

        [Column("ApprovedAt")]
        public DateTime? ApprovedAt { get; set; }
    }
}