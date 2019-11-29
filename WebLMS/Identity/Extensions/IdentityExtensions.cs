using System.Security.Claims;
using System.Security.Principal;

namespace WebLMS.Identity.Extensions
{
    public static class IdentityExtensions
    {
        public static string GetStudentName(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("StudentName");
            // Test for null to avoid issues during local testing
            return claim != null ? claim.Value : string.Empty;
        }
    }
}
