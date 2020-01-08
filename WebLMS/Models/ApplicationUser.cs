using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebLMS.Models
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        [StringLength(30, MinimumLength = 5)]
        public string StudentName { get; set; }

        [PersonalData]
        [StringLength(20, MinimumLength = 3)]
        public string Group { get; set; }
    }
}
