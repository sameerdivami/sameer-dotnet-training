using System.ComponentModel.DataAnnotations;

  namespace PolicyManagement.DTOs
{
    public class UserResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Role { get; set; } = null!;
    }

    public class UserCreateDto
    {
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        public string Password { get; set; } = null!;

        public string Role { get; set; } = "User";
    }
}

