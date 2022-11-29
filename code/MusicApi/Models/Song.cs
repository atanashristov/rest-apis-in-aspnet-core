using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

namespace MusicApi.Models
{
  public class Song
  {
    public Guid Id { get; set; }

    [StringLength(200, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 200 characters")]
    [Required(ErrorMessage = "Title cannot be empty")]
    public string Title { get; set; } = "";

    // [StringLength(60, MinimumLength = 2, ErrorMessage = "Language name must be between 2 and 60 characters")]
    // [Required(ErrorMessage = "Language cannot be empty")]
    // public string? Language { get; set; }

    [DisplayName("Duration seconds")]
    [Range(0, int.MaxValue, ErrorMessage = "Duration must be a positive number")]
    [Required(ErrorMessage = "Duration cannot be empty")]
    public int? Duration { get; set; }

    public DateTime UploadedDate { get; set; } = DateTime.Now.ToUniversalTime();

    public bool IsFeatured { get; set; } = false;

    [NotMapped]
    [XmlIgnore]
    public IFormFile? ImageFile { get; set; }

    public string? ImageUrl { get; set; }

    [NotMapped]
    [XmlIgnore]
    public IFormFile? AudioFile { get; set; }

    public string? AudioUrl { get; set; }

    public Guid ArtistId { get; set; }

    public Guid? AlbumId { get; set; }
  }
}