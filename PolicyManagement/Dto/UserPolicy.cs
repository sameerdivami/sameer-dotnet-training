namespace PolicyManagement.DTOs
{
    public class UserPolicyDetailsDto
    {
        public string? PolicyName { get; set; }
        public int PremiumAmount { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public string? Status { get; set; }
    }
}