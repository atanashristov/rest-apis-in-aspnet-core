using System.ComponentModel.DataAnnotations.Schema;

namespace MusicApi.Models
{
  public class Song
  {
    public Guid Id { get; set; }
    public string Title { get; set; } = "";
    public string? Language { get; set; }
    public int? Duration { get; set; }
    [NotMapped]
    public IFormFile? Image { get; set; }
    public String? ImageUrl { get; set; }
  }
}