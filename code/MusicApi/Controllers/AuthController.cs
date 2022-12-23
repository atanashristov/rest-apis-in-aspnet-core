using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using MusicApi.Data;
using MusicApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using MusicApi.Services.UserService;
using System.Security.Cryptography;

namespace MusicApi.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class AuthController : ControllerBase
  {
    private readonly IUserService _userService;
    private readonly MusicApiDbContext _db;
    private readonly ILogger<AuthController> _logger;
    private readonly IConfiguration _configuration;

    public AuthController(MusicApiDbContext db, IUserService userService, ILogger<AuthController> logger, IConfiguration configuration)
    {
      _userService = userService ?? throw new ArgumentNullException(nameof(userService));
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
        return StatusCode(StatusCodes.Status422UnprocessableEntity, "Invalid login");
      }

      if (!VerifyPasswordHash(user.Password, dbUser.PasswordHash, dbUser.PasswordSalt))
      {
        return StatusCode(StatusCodes.Status422UnprocessableEntity, "Invalid login");
      }

      string token = CreateToken(dbUser.Id, dbUser.Username);
      var refreshToken = await GenerateRefreshToken(dbUser.Id);
      SetRefreshTokenHttpOnlyCookie(refreshToken);

      return Ok(System.Text.Json.JsonSerializer.Serialize(token));
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken()
    {
      var refreshTokenFromCookie = Request.Cookies["refreshToken"];
      var dbRefreshToken = _db.RefreshTokens.FirstOrDefault(x => x.Token == refreshTokenFromCookie);

      if (dbRefreshToken == null)
      {
        return Unauthorized("Invalid refresh token");
      }
      if (dbRefreshToken.ExpiresAt <= DateTime.Now.ToUniversalTime())
      {
        return Unauthorized("Token expired");
      }

      var dbUser = _db.Users.FirstOrDefault(dbUser => dbUser.Id == dbRefreshToken.UserId);
      if (dbUser == null)
      {
        return StatusCode(StatusCodes.Status422UnprocessableEntity, "Invalid user");
      }

      string token = CreateToken(dbUser.Id, dbUser.Username);
      var refreshToken = await GenerateRefreshToken(dbUser.Id);
      SetRefreshTokenHttpOnlyCookie(refreshToken);

      return Ok(System.Text.Json.JsonSerializer.Serialize(token));
    }

    [HttpGet("me"), Authorize]
    public async Task<IActionResult> GetMe()
    {
      // Using a service with injected IHttpContextAccessor to resolve the ClaimPrincipal
      // Also, the controller method or the controller class has to be annotated with `AuthorizeAttribute`.
      var identityUserId = _userService.GetMyUserId();
      if (identityUserId == null)
      {
        return BadRequest("User not found");
      }

      // Access to ClaimPrincipal directly from within the controller.
      // var identityId = User?.FindFirstValue(ClaimTypes.NameIdentifier);
      // if (!Guid.TryParse(identityId, out Guid identityUserId))
      // {
      //   return BadRequest("User not found");
      // }

      var identityName = User?.FindFirstValue(ClaimTypes.Name); // Same as `User?.Identity?.Name`
      var identityIsAdmin = User?.IsInRole("Admin") ?? false;


      var dbUser = await _db.Users.FindAsync(identityUserId);
      if (dbUser == null)
      {
        return StatusCode(StatusCodes.Status422UnprocessableEntity, "User not found");
      }

      return Ok(new { dbUser.Id, dbUser.Username, dbUser.EmailAddress, identityUserId, identityName, identityIsAdmin });
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

    private string CreateToken(Guid userId, string? userName)
    {
      // Claims:
      // - stored in the token
      // - describe the user that is authenticate`
      // - can have the user ID

      var claims = new List<Claim>
      {
        new Claim(ClaimTypes.NameIdentifier, $"{userId}"),
        new Claim(ClaimTypes.Name, $"{userName}"),
        new Claim(ClaimTypes.Role, "Role1"),
        new Claim(ClaimTypes.Role, "Admin"),
      };

      var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
        _configuration.GetSection("AppSettings:Token").Value ?? ""));

      var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

      var token = new JwtSecurityToken(
        claims: claims,
        expires: DateTime.Now.AddMinutes(5),
        signingCredentials: credentials);

      var jwt = new JwtSecurityTokenHandler().WriteToken(token);

      return jwt;
    }

    private async Task<RefreshToken> GenerateRefreshToken(Guid userId)
    {
      var refreshToken = _db.RefreshTokens
        .Where(x => x.UserId == userId && x.ExpiresAt >= DateTime.Now.ToUniversalTime())
        .OrderByDescending(x => x.ExpiresAt)
        .FirstOrDefault();

      if (refreshToken == null)
      {
        refreshToken = new RefreshToken
        {
          Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
          UserId = userId,
        };

        await _db.RefreshTokens.AddAsync(refreshToken);
        await _db.SaveChangesAsync();
      }

      return refreshToken;
    }

    private void SetRefreshTokenHttpOnlyCookie(RefreshToken refreshToken)
    {
      var cookieOptions = new CookieOptions
      {
        HttpOnly = true,
        Expires = refreshToken.ExpiresAt,
      };

      Response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);
    }
  }
}