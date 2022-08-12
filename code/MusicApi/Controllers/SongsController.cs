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
    public IActionResult Get()
    {
      return Ok(_db.Songs);
    }

    [HttpGet("{id}")]
    public IActionResult Get(Guid id)
    {
      var song = _db.Songs?.Find(id);
      if (song == null)
      {
        return NotFound("No song found with id " + id);
      }

      return Ok(song);
    }

    [HttpPost]
    public IActionResult Post([FromBody] Song song)
    {
      _db.Songs?.Add(song);
      _db.SaveChanges();
      return StatusCode(StatusCodes.Status201Created, song);
    }

    [HttpPut("{id}")]
    public IActionResult Put(Guid id, [FromBody] Song song)
    {
      var dbSong = _db.Songs?.Find(id);
      if (dbSong == null)
      {
        return NotFound("No song found with id " + id);
      }

      dbSong.Title = song.Title;
      dbSong.Language = song.Language;
      _db.SaveChanges();
      return Ok(dbSong);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
      var dbSong = _db.Songs?.Find(id);
      if (dbSong == null)
      {
        return NotFound("No song found with id " + id);
      }

      _db.Songs?.Remove(dbSong);
      _db.SaveChanges();
      return Ok("Record deleted");
    }
  }
}