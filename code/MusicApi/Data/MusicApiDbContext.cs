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
      Albums = Set<Album>();
      Artists = Set<Artist>();
      Users = Set<User>();
    }

    public DbSet<Song> Songs { get; set; }
    public DbSet<Album> Albums { get; set; }
    public DbSet<Artist> Artists { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      // One way to set connection string
      // optionsBuilder.UseNpgsql(@"Host=localhost;Username=musicapi;Password=musicapi;Database=music");
    }

    // Data seeding:
    // https://docs.microsoft.com/en-us/ef/core/miscellaneous/configuring-dbcontext
    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
    //   modelBuilder.Entity<Song>()
    //     .HasData(
    //       new Song { Id = Guid.Parse("{00000000-0000-0000-0000-000000000001}"), Title = "Song 1", Duration = 120 },
    //       new Song { Id = Guid.Parse("{00000000-0000-0000-0000-000000000002}"), Title = "Song 2", Duration = 240 },
    //       new Song { Id = Guid.Parse("{00000000-0000-0000-0000-000000000003}"), Title = "Song 3", Duration = 360 },
    //       new Song { Id = Guid.Parse("{00000000-0000-0000-0000-000000000004}"), Title = "Song 4", Duration = 480 },
    //       new Song { Id = Guid.Parse("{00000000-0000-0000-0000-000000000005}"), Title = "Song 5", Duration = 600 },
    //       new Song { Id = Guid.Parse("{00000000-0000-0000-0000-000000000006}"), Title = "Song 6", Duration = 720 },
    //       new Song { Id = Guid.Parse("{00000000-0000-0000-0000-000000000007}"), Title = "Song 7", Duration = 840 },
    //       new Song { Id = Guid.Parse("{00000000-0000-0000-0000-000000000008}"), Title = "Song 8", Duration = 960 },
    //       new Song { Id = Guid.Parse("{00000000-0000-0000-0000-000000000009}"), Title = "Song 9", Duration = 1080 },
    //       new Song { Id = Guid.Parse("{00000000-0000-0000-0000-000000000010}"), Title = "Song 10", Duration = 1200 },
    //       new Song { Id = Guid.Parse("{00000000-0000-0000-0000-000000000011}"), Title = "Song 11", Duration = 1320 },
    //       new Song { Id = Guid.Parse("{00000000-0000-0000-0000-000000000012}"), Title = "Song 12", Duration = 1440 },
    //       new Song { Id = Guid.Parse("{00000000-0000-0000-0000-000000000013}"), Title = "Song 13", Duration = 1560 }
    //     );
    // }
  }
}