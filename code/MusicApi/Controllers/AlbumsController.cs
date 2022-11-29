using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicApi.Data;
using MusicApi.Helpers;
using MusicApi.Models;

namespace MusicApi.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class AlbumsController : ControllerBase
  {
    private readonly MusicApiDbContext _db;
    private readonly ILogger<AlbumsController>? _logger;
    private readonly IFormFileUploader _formFileUploader;

    public AlbumsController(MusicApiDbContext db, ILogger<AlbumsController> logger, IFormFileUploader formFileUploader)
    {
      _db = db ?? throw new ArgumentNullException(nameof(db));
      _logger = logger ?? throw new ArgumentNullException(nameof(logger));
      _formFileUploader = formFileUploader ?? throw new ArgumentNullException(nameof(formFileUploader));
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromForm] Album album)
    {
      if (album.ImageFile != null)
      {
        album.ImageUrl = await _formFileUploader.UploadFormFile(album.ImageFile);
      }

      await _db.Albums.AddAsync(album);
      await _db.SaveChangesAsync();
      return StatusCode(StatusCodes.Status201Created, album.Id);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAlbums()
    {
      var albums = await (from album in _db.Albums
                          orderby album.Id
                          select new
                          {
                            album.Id,
                            album.Name,
                            album.ImageUrl,
                          }).ToListAsync();

      return Ok(albums);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAlbumDetails(Guid id)
    {
      var album = await _db.Albums
        .Where(album => album.Id == id)
        .Select((album) => new
        {
          album.Id,
          album.Name,
          album.ImageUrl,
          Songs = album.Songs.AsEnumerable()
            .Select(song => new
            {
              song.Id,
              song.Title,
              song.Duration,
              song.IsFeatured,
              song.ImageUrl,
              song.AudioUrl,
            }),
        }).FirstOrDefaultAsync();

      return Ok(album);
    }

  }
}