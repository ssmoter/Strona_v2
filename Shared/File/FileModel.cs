using System.ComponentModel.DataAnnotations;

namespace Strona_v2.Shared.File
{
    public class FileModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Tytuł jest wymagany")]
        [StringLength(100)]
        public string? Title { get; set; }
        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        public string? Type { get; set; }

        public string? User { get; set; }
        public int UserId { get; set; }

        [Required]
        public string? Path { get; set; }

        public int Like { get; set; }

        public int Spam { get; set; }

        public DateTimeOffset? Created { get; set; }

        public bool Main { get;set; }
    }
}
