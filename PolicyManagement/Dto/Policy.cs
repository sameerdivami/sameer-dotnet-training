using System.ComponentModel.DataAnnotations;

  namespace PolicyManagement.DTOs
{
    public class ReturnPolicyDto
    {
        public string Name { get; set; } = null!;
        public int PremiumAmount { get; set; }
        public string Description { get; set; } = null!;
        public bool IsActive { get; set; }
    }

    public class CreatePolicyDto
    {
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Premium amount must be a positive number")]
        public int PremiumAmount { get; set; }

        [Required]
        public string Description { get; set; } = null!;

        [Required]
        public bool IsActive { get; set; }
    }
}