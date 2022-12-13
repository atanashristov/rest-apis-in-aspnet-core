using System.Security.Claims;

namespace MusicApi.Services.UserService
{
  public class UserService : IUserService
  {
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(IHttpContextAccessor httpContextAccessor)
    {
      _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }

    public Guid? GetMyUserId()
    {
      var claimsPrincipal = _httpContextAccessor.HttpContext?.User;
      if (claimsPrincipal != null)
      {
        var identityId = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
        if (Guid.TryParse(identityId, out Guid userId))
        {
          return userId;
        }
      }

      return null;
    }
  }
}