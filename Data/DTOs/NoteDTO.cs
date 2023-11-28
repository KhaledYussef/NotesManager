using Microsoft.AspNetCore.Http;

using System.ComponentModel.DataAnnotations;

namespace Data.DTOs
{
    public sealed class NoteDTO
    {
        /// <example>0</example>
        public int Id { get; set; }

        /// <example>My Note Title</example>
        [Required]
        [StringLength(250)]
        public string Title { get; set; }


        /// <example>My Note Content</example>
        [StringLength(5000)]
        public string Content { get; set; }

        /// <example>#FFFFFF</example>
        [StringLength(10)]
        public string Color { get; set; } = "#FFFFFF";


        [StringLength(500)]
        public string ImageUrl { get; set; }


        public IFormFile ImageFile { get; set; }


    }
}
