using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Xml.Serialization;
using Microsoft.EntityFrameworkCore;

namespace MusicApi.Models
{
  [Index(nameof(Username), IsUnique = false)]
  [Index(nameof(EmailAddress), IsUnique = true)]
  public class User
  {
    public Guid Id { get; set; }

    [StringLength(256, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 256 characters")]
    public string? Username { get; set; }

    [StringLength(256, MinimumLength = 3, ErrorMessage = "EmailAddress must be between 3 and 256 characters")]
    [Required(ErrorMessage = "EmailAddress cannot be empty")]
    [EmailAddress]
    public string EmailAddress { get; set; } = String.Empty;

    [StringLength(256, MinimumLength = 3, ErrorMessage = "Password must be between 3 and 256 characters")]
    [Required(ErrorMessage = "Password cannot be empty")]
    [NotMapped]
    public string Password { get; set; } = String.Empty;

    [XmlIgnore]
    [JsonIgnore]
    public byte[] PasswordHash { get; set; } = new byte[0];

    [XmlIgnore]
    [JsonIgnore]
    public byte[] PasswordSalt { get; set; } = new byte[0];

    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
  }
}