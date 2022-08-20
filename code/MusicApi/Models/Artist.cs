using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicApi.Models
{
  public class Artist
  {
    public Guid Id { get; set; }

    [StringLength(200, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 200 characters")]
    [Required(ErrorMessage = "Name cannot be empty")]
    public string Name { get; set; } = "";

    [StringLength(60, MinimumLength = 2, ErrorMessage = "Gender name must be between 2 and 60 characters")]
    public string? Gender { get; set; }

    [NotMapped]
    public IFormFile? Image { get; set; }

    public string? ImageUrl { get; set; }

    public ICollection<Album> Albums { get; set; } = new List<Album>();
    public ICollection<Song> Songs { get; set; } = new List<Song>();

  }
}