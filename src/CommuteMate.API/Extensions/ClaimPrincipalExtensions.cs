using System.Security.Claims;

namespace CommuteMate.API.Extensions
{
    public static class ClaimPrincipalExtensions
    {
        public static int GetUserId(this ClaimsPrincipal user)
        {
            return int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);
        }
    }
}
