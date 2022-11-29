using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

namespace MusicApi.Models
{
  public class Album
  {
    public Guid Id { get; set; }

    [StringLength(200, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 200 characters")]
    [Required(ErrorMessage = "Name cannot be empty")]
    public string Name { get; set; } = "";

    [NotMapped]
    [XmlIgnore]
    public IFormFile? ImageFile { get; set; }

    public string? ImageUrl { get; set; }

    public Guid ArtistId { get; set; }
    public ICollection<Song> Songs { get; set; } = new List<Song>();
  }
}