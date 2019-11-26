using Microsoft.AspNetCore.Identity;

namespace WebLMS.Models
{
    public class ApplicationUser : IdentityUser
    {
        string Name { get; set; }
    }
}
