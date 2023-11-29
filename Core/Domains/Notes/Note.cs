using Core.Domains.Auth;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domains.Notes
{
    public class Note : BaseEntity<int>
    {
        [Required]
        [StringLength(250)]
        public string Title { get; set; }


        [StringLength(5000)]
        public string Content { get; set; }

        [StringLength(10)]
        public string Color { get; set; } = "#FFFFFF";

        [StringLength(500)]
        public string ImageUrl { get; set; }

        [Required]
        public string UserId { get; set; }

        public bool IsShared { get; set; }

        public string ShareLink { get; set; }


        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser User { get; set; }
    }
}
