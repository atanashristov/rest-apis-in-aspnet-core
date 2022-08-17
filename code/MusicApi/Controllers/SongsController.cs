using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicApi.Data;
using MusicApi.Helpers;
using MusicApi.Models;

namespace MusicApi.Controllers
{
  [Route("api/[controller]")]
  public class SongsController : ControllerBase
  {
    private readonly MusicApiDbContext _db;
    private readonly ILogger<SongsController> _logger;
    private readonly IFormFileUploader _formFileUploader;

    public SongsController(MusicApiDbContext db, ILogger<SongsController> logger, IFormFileUploader formFileUploader)
    {
      _db = db ?? throw new ArgumentNullException(nameof(db));
      _logger = logger ?? throw new ArgumentNullException(nameof(logger));
      _formFileUploader = formFileUploader ?? throw new ArgumentNullException(nameof(formFileUploader));
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

    // [HttpPost]
    // public async Task<IActionResult> Post([FromBody] Song song)
    // {
    //   await _db.Songs.AddAsync(song);
    //   await _db.SaveChangesAsync();

    //   return StatusCode(StatusCodes.Status201Created, song.Id);
    // }

    [HttpPost]
    public async Task<IActionResult> Post([FromForm] Song song)
    {
      song.ImageUrl = await _formFileUploader.UploadFormFile(song.Image);

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