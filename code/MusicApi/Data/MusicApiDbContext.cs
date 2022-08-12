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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Song>()
        .HasData(
          new Song { Id = Guid.Parse("{00000000-0000-0000-0000-000000000001}"), Title = "Song 1", Language = "en", Duration = 120 },
          new Song { Id = Guid.Parse("{00000000-0000-0000-0000-000000000002}"), Title = "Song 2", Language = "en", Duration = 240 },
          new Song { Id = Guid.Parse("{00000000-0000-0000-0000-000000000003}"), Title = "Song 3", Language = "en", Duration = 360 },
          new Song { Id = Guid.Parse("{00000000-0000-0000-0000-000000000004}"), Title = "Song 4", Language = "en", Duration = 480 },
          new Song { Id = Guid.Parse("{00000000-0000-0000-0000-000000000005}"), Title = "Song 5", Language = "en", Duration = 600 },
          new Song { Id = Guid.Parse("{00000000-0000-0000-0000-000000000006}"), Title = "Song 6", Language = "en", Duration = 720 },
          new Song { Id = Guid.Parse("{00000000-0000-0000-0000-000000000007}"), Title = "Song 7", Language = "en", Duration = 840 },
          new Song { Id = Guid.Parse("{00000000-0000-0000-0000-000000000008}"), Title = "Song 8", Language = "en", Duration = 960 },
          new Song { Id = Guid.Parse("{00000000-0000-0000-0000-000000000009}"), Title = "Song 9", Language = "en", Duration = 1080 },
          new Song { Id = Guid.Parse("{00000000-0000-0000-0000-000000000010}"), Title = "Song 10", Language = "en", Duration = 1200 },
          new Song { Id = Guid.Parse("{00000000-0000-0000-0000-000000000011}"), Title = "Song 11", Language = "en", Duration = 1320 },
          new Song { Id = Guid.Parse("{00000000-0000-0000-0000-000000000012}"), Title = "Song 12", Language = "en", Duration = 1440 },
          new Song { Id = Guid.Parse("{00000000-0000-0000-0000-000000000013}"), Title = "Song 13", Language = "en", Duration = 1560 }
        );
    }
  }
}