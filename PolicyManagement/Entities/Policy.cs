using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace PolicyManagement.Entities
{
    [Table("Policy")]
    public class Policy
    {
         [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [Column("PolicyName")]
        [Required(ErrorMessage = "Policy name is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Policy name must be between 3 and 100 characters")]
        public required string PolicyName { get; set; }

        [Column("PremiumAmount")]
        [Required(ErrorMessage = "Premium amount is required")]
        [Range(1, 100000, ErrorMessage = "Premium amount must be greater than 0")]
        public int PremiumAmount { get; set; }

        [Column("Description")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }

        [Column("IsActive")]
        public bool IsActive { get; set; }

        [Column("CreatedAt")]
        public DateTime CreatedAt { get; set; }

        [Column("UpdatedAt")]
        public DateTime UpdatedAt { get; set; }
    }
}