using Microsoft.AspNetCore.Identity;

using System.ComponentModel.DataAnnotations;

namespace Core.Domains.Auth
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(250)]
        public string FullName { get; set; }
    }
}
