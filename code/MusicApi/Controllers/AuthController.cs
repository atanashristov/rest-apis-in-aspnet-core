using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using MusicApi.Data;
using MusicApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace MusicApi.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class AuthController : ControllerBase
  {
    private readonly MusicApiDbContext _db;
    private readonly ILogger<AuthController> _logger;
    private readonly IConfiguration _configuration;

    public AuthController(MusicApiDbContext db, ILogger<AuthController> logger, IConfiguration configuration)
    {
      _db = db ?? throw new ArgumentNullException(nameof(db));
      _logger = logger ?? throw new ArgumentNullException(nameof(logger));
      _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromForm] User user)
    {
      user.EmailAddress = user.EmailAddress.ToLowerInvariant();

      if (_db.Users.Any(x => x.EmailAddress == user.EmailAddress))
      {
        return StatusCode(StatusCodes.Status409Conflict, "User already exists");
      }

      CreatePasswordHash(user.Password, out byte[] passwordHash, out byte[] passwordSalt);
      user.Password = string.Empty;
      user.PasswordHash = passwordHash;
      user.PasswordSalt = passwordSalt;

      await _db.Users.AddAsync(user);
      await _db.SaveChangesAsync();
      return StatusCode(StatusCodes.Status201Created, user.Id);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromForm] User user)
    {
      user.EmailAddress = user.EmailAddress.ToLowerInvariant();

      var dbUser = _db.Users.FirstOrDefault(dbUser => dbUser.EmailAddress == user.EmailAddress);
      if (dbUser == null)
      {
        return BadRequest("Invalid login");
      }

      if (!VerifyPasswordHash(user.Password, dbUser.PasswordHash, dbUser.PasswordSalt))
      {
        return BadRequest("Invalid login");
      }

      await Task.FromResult(1);
      string token = CreateToken(dbUser);
      return Ok(new { token });
    }


    private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
      using (var hmac = new System.Security.Cryptography.HMACSHA512())
      {
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
      }
    }

    private static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
      using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
      {
        var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        return computedHash.SequenceEqual(passwordHash);
      }
    }

    private string CreateToken(User user)
    {
      // Claims:
      // - stored in the token
      // - describe the user that is authenticate`
      // - can have the user ID

      var claims = new List<Claim>
      {
        new Claim(ClaimTypes.Name, user.Id.ToString()),
      };

      var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
        _configuration.GetSection("AppSettings:Token").Value ?? ""));

      var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

      var token = new JwtSecurityToken(
        claims: claims,
        expires: DateTime.Now.AddDays(1),
        signingCredentials: credentials);

      var jwt = new JwtSecurityTokenHandler().WriteToken(token);

      return jwt;
    }
  }
}