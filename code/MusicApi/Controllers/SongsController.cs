using Microsoft.AspNetCore.Mvc;
using MusicApi.Models;

namespace MusicApi.Controllers
{
  [Route("api/[controller]")]
  public class SongsController : Controller
  {
    private IList<Song> _songs = new List<Song>
    {
      new Song { Id = Guid.Parse("26f16538-8fed-4c9f-a559-d8750c15b477"), Title = "Song 1", Language = "English" },
      new Song { Id = Guid.Parse("7c340bb5-0200-49ef-a5de-6eed7ed2a522"), Title = "Song 2", Language = "German" },
    };

    private readonly ILogger<SongsController> _logger;

    public SongsController(ILogger<SongsController> logger)
    {
      _logger = logger;
    }

    [HttpGet]
    public IEnumerable<Song> Get()
    {
      Console.WriteLine($"Hello World from {System.Diagnostics.Process.GetCurrentProcess().Id}");
      return _songs;
    }

    [HttpGet("{id}")]
    public Song? Get(Guid id)
    {
      return _songs.FirstOrDefault(s => s.Id == id);
    }

    [HttpPost]
    public void Post([FromBody] Song song)
    {
      _songs.Add(song);
    }

    [HttpPut("{id}")]
    public void Put(Guid id, [FromBody] Song song)
    {
      var songToUpdate = _songs.FirstOrDefault(s => s.Id == id);
      // TODO: not found? Bad request?
      if (songToUpdate != null)
      {
        songToUpdate.Title = song.Title;
        songToUpdate.Language = song.Language;
      }
    }

    [HttpDelete("{id}")]
    public void Delete(Guid id)
    {
      var songToDelete = _songs.FirstOrDefault(s => s.Id == id);
      // TODO: not found? Bad request?
      if (songToDelete != null)
      {
        _songs.Remove(songToDelete);
      }
    }
  }
}