using Microsoft.AspNetCore.Mvc;
using MusicApi.Data;
using MusicApi.Models;

namespace MusicApi.Controllers
{
  [Route("api/[controller]")]
  public class SongsController : ControllerBase
  {
    private readonly MusicApiDbContext _db;
    private readonly ILogger<SongsController> _logger;

    public SongsController(MusicApiDbContext db, ILogger<SongsController> logger)
    {
      _db = db;
      _logger = logger;
    }

    [HttpGet]
    public IEnumerable<Song>? Get()
    {
      return _db.Songs;
    }

    [HttpGet("{id}")]
    public Song? Get(Guid id)
    {
      var song = _db.Songs?.Find(id);
      return song;
    }

    [HttpPost]
    public void Post([FromBody] Song song)
    {
      _db.Songs?.Add(song);
      _db.SaveChanges();
    }

    [HttpPut("{id}")]
    public void Put(Guid id, [FromBody] Song song)
    {
      var dbSong = _db.Songs?.Find(id);
      if (dbSong != null)
      {
        dbSong.Title = song.Title;
        dbSong.Language = song.Language;
        _db.SaveChanges();
      }
    }

    [HttpDelete("{id}")]
    public void Delete(Guid id)
    {
      var dbSong = _db.Songs?.Find(id);
      if (dbSong != null)
      {
        _db.Songs?.Remove(dbSong);
        _db.SaveChanges();
      }
    }
  }
}