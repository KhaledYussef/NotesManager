using System.ComponentModel.DataAnnotations;

namespace Data.DTOs
{
    public sealed class RegisterDTO
    {
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(250)]
        public string Email { get; set; }

        [Required]
        [MaxLength(250)]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password))]
        [MaxLength(250)]
        public string ConfirmPassword { get; set; }
    }


    public sealed class LoginDTO
    {

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public bool RememberMe { get; set; }

    }
}
