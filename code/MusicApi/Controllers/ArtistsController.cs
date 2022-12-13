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
  public class ArtistsController : ControllerBase
  {
    private readonly MusicApiDbContext _db;
    private readonly ILogger<ArtistsController> _logger;
    private readonly IFormFileUploader _formFileUploader;

    public ArtistsController(MusicApiDbContext db, ILogger<ArtistsController> logger, IFormFileUploader formFileUploader)
    {
      _db = db ?? throw new ArgumentNullException(nameof(db));
      _logger = logger ?? throw new ArgumentNullException(nameof(logger));
      _formFileUploader = formFileUploader ?? throw new ArgumentNullException(nameof(formFileUploader));
    }

    [HttpPost, Authorize(Roles = "Admin")]
    public async Task<IActionResult> Post([FromForm] Artist artist)
    {
      if (artist.ImageFile != null)
      {
        artist.ImageUrl = await _formFileUploader.UploadFormFile(artist.ImageFile);
      }

      await _db.Artists.AddAsync(artist);
      await _db.SaveChangesAsync();
      return StatusCode(StatusCodes.Status201Created, artist.Id);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllArtists()
    {
      var artists = await (from artist in _db.Artists
                           orderby artist.Id
                           select new
                           {
                             artist.Id,
                             artist.Name,
                             artist.ImageUrl,
                           }).ToListAsync();

      return Ok(artists);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetArtistDetails(Guid id)
    {
      // var artist = await _db.Artists.FindAsync(id);
      var artist = await _db.Artists
        .Where(artist => artist.Id == id)//.Include(a => a.Albums)
        .Select((artist) => new
        {
          artist.Id,
          artist.Name,
          artist.ImageUrl,
          Albums = artist.Albums.AsEnumerable()
            .Select(album => new { album.Id, album.Name, album.ImageUrl, }),
        })
        .FirstOrDefaultAsync();

      if (artist == null)
      {
        return NotFound("Artist not found");
      }

      return Ok(artist);
    }

    [HttpPut("{id}"), Authorize(Roles = "Admin")]
    public async Task<IActionResult> Put(Guid id, [FromBody] Artist artist)
    {
      var dbArtist = await _db.Artists.FindAsync(id);
      if (dbArtist == null)
      {
        return NotFound("Artist not found");
      }

      dbArtist.Name = artist.Name;
      await _db.SaveChangesAsync();

      return Ok();
    }

    [HttpDelete("{id}"), Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
      var dbArtist = await _db.Artists.FindAsync(id);
      if (dbArtist == null)
      {
        return NotFound("Artist not found");
      }

      _db.Artists.Remove(dbArtist);
      await _db.SaveChangesAsync();

      return Ok("Artist deleted");
    }
  }
}