using Microsoft.EntityFrameworkCore;

namespace MusicApi.Models
{
  [Index(nameof(Token), IsUnique = false)]
  [Index(nameof(CreatedAt), IsUnique = false)]
  [Index(nameof(ExpiresAt), IsUnique = false)]
  public class RefreshToken
  {
    public Guid Id { get; set; }
    public string Token { get; set; } = String.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime ExpiresAt { get; set; } = DateTime.Now.AddDays(4);
    public DateTime? RedeemedAt { get; set; }
    public Guid UserId { get; set; }
  }
}