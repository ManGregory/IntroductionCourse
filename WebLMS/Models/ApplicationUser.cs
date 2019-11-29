using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebLMS.Models
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        public string StudentName { get; set; }

        [PersonalData]
        public string Group { get; set; }
    }
}
