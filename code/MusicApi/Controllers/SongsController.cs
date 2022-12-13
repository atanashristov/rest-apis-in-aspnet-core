using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicApi.Data;
using MusicApi.Helpers;
using MusicApi.Models;

namespace MusicApi.Controllers
{
  [ApiController]
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

    [HttpPost, Authorize(Roles = "Admin")]
    // NOTE: Validation using filter attribute.
    // [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> Post([FromForm] Song song)
    {
      // NOTE: Validation using ModelState
      // if (!ModelState.IsValid)
      // {
      //   return UnprocessableEntity(ModelState);
      // }

      if (song.ImageFile != null)
      {
        song.ImageUrl = await _formFileUploader.UploadFormFile(song.ImageFile);
      }

      if (song.AudioFile != null)
      {
        song.AudioUrl = await _formFileUploader.UploadFormFile(song.AudioFile);
      }

      await _db.Songs.AddAsync(song);
      await _db.SaveChangesAsync();
      return StatusCode(StatusCodes.Status201Created, song.Id);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllSongs(int pageNumber, int pageSize)
    {
      var songs = await (from song in _db.Songs
                         orderby song.Id
                         select new
                         {
                           song.Id,
                           song.Title,
                           song.Duration,
                           song.IsFeatured,
                           song.ImageUrl,
                           song.AudioUrl,
                         })
                         .Skip((pageNumber - 1) * pageSize).Take(pageSize)
                         .ToListAsync();
      return Ok(songs);
    }

    [HttpGet("featured")]
    public async Task<IActionResult> GetFeaturedSongs()
    {
      var songs = await (from song in _db.Songs
                         where song.IsFeatured
                         select new
                         {
                           song.Id,
                           song.Title,
                           song.Duration,
                           song.IsFeatured,
                           song.ImageUrl,
                           song.AudioUrl,
                         }).Take(10).ToListAsync();
      return Ok(songs);
    }

    [HttpGet("new")]
    public async Task<IActionResult> GetNewSongs()
    {
      var songs = await (from song in _db.Songs
                         orderby song.UploadedDate descending
                         select new
                         {
                           song.Id,
                           song.Title,
                           song.Duration,
                           song.IsFeatured,
                           song.ImageUrl,
                           song.AudioUrl,
                         }).Take(10).ToListAsync();
      return Ok(songs);
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchSongs(string query)
    {
      var songs = await (from song in _db.Songs
                         where song.Title.ToLower().StartsWith(query.ToLowerInvariant())
                         select new
                         {
                           song.Id,
                           song.Title,
                           song.Duration,
                           song.IsFeatured,
                           song.ImageUrl,
                           song.AudioUrl,
                         }).Take(10).ToListAsync();
      return Ok(songs);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSongDetails(Guid id)
    {
      var song = await _db.Songs.FindAsync(id);

      if (song == null)
      {
        return NotFound("Song not found");
      }

      return Ok(song);
    }


    // Attribute routing example
    // /api/songs/test/1 -> 1
    // [HttpGet("[action]/{id}")]
    // public IActionResult Test(int id)
    // {
    //   return Ok(id);
    // }


    [HttpPut("{id}"), Authorize(Roles = "Admin")]
    public async Task<IActionResult> Put(Guid id, [FromBody] Song song)
    {
      var dbSong = await _db.Songs.FindAsync(id);
      if (dbSong == null)
      {
        return NotFound("Song not found");
      }

      dbSong.Title = song.Title;
      // dbSong.Language = song.Language;
      dbSong.Duration = song.Duration;
      await _db.SaveChangesAsync();

      return Ok();
    }

    [HttpDelete("{id}"), Authorize(Roles = "Admin")]
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