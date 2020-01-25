using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;
using WebLMS.Models;

namespace WebLMS.Identity
{
    public class IdentityUtils
    {
        public UserManager<ApplicationUser> UserManager { get; set; }
        public ClaimsPrincipal CurrentUser { get; set; }

        public IdentityUtils(UserManager<ApplicationUser> manager, ClaimsPrincipal currentUser)
        {
            UserManager = manager;
            CurrentUser = currentUser;
        }

        public async Task<bool> IsUserCanViewOtherUsers(string userEmail)
        {
            if (string.IsNullOrEmpty(userEmail)) return true;

            return await IsCurrentUserAdmin();
        }

        private async Task<bool> IsCurrentUserAdmin()
        {
            var user = await GetCurrentUser();
            return await UserManager.IsInRoleAsync(user, "Administrator");
        }

        private async Task<ApplicationUser> GetCurrentUser()
        {
            return await UserManager.GetUserAsync(CurrentUser);
        }

        public async Task<ApplicationUser> GetUser(string userEmail)
        {
            if (string.IsNullOrEmpty(userEmail)) return await GetCurrentUser();
            return await UserManager.FindByEmailAsync(userEmail);
        }
    }
}
