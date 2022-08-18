using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicApi.Models
{
  public class Song
  {
    public Guid Id { get; set; }

    [StringLength(200, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 60 characters")]
    [Required(ErrorMessage = "Title cannot be empty")]
    public string Title { get; set; } = "";

    [StringLength(60, MinimumLength = 2, ErrorMessage = "Language name must be between 2 and 60 characters")]
    [Required(ErrorMessage = "Language cannot be empty")]
    public string? Language { get; set; }

    [DisplayName("Duration seconds")]
    [Range(0, int.MaxValue, ErrorMessage = "Duration must be a positive number")]
    [Required(ErrorMessage = "Duration cannot be empty")]
    public int? Duration { get; set; }

    [NotMapped]
    public IFormFile? Image { get; set; }

    public String? ImageUrl { get; set; }
  }
}