using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public async Task<IActionResult> Get()
    {
      return Ok(await _db.Songs.ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
      var song = await _db.Songs.FindAsync(id);
      if (song == null)
      {
        return NotFound("Song not found");
      }

      return Ok(song);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Song song)
    {
      await _db.Songs.AddAsync(song);
      await _db.SaveChangesAsync();

      return StatusCode(StatusCodes.Status201Created, song.Id);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(Guid id, [FromBody] Song song)
    {
      var dbSong = await _db.Songs.FindAsync(id);
      if (dbSong == null)
      {
        return NotFound("Song not found");
      }

      dbSong.Title = song.Title;
      dbSong.Language = song.Language;
      dbSong.Duration = song.Duration;
      await _db.SaveChangesAsync();

      return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
      var dbSong = await _db.Songs.FindAsync(id);
      if (dbSong == null)
      {
        return NotFound("Song not found");
      }

      _db.Songs.Remove(dbSong);
      await _db.SaveChangesAsync();

      return Ok("Song deleted");
    }
  }
}