using Microsoft.AspNet.Identity;
using System.Security.Claims;

namespace ChaatyApi.Helpers
{
    public static class IdentityExtention
    {
        public static string GetTheUserID(this ClaimsPrincipal user)
        {
            return user.Identity.GetUserId();
        }
    }
}
