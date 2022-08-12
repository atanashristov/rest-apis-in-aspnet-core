using Microsoft.EntityFrameworkCore;
using MusicApi.Models;

namespace MusicApi.Data
{
  public class MusicApiDbContext : DbContext
  {
    public MusicApiDbContext(DbContextOptions<MusicApiDbContext> options)
      : base(options)
    {
      Songs = Set<Song>();
    }

    public DbSet<Song> Songs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      // optionsBuilder.UseNpgsql(@"Host=localhost;Username=musicapi;Password=musicapi;Database=music");
    }
  }
}