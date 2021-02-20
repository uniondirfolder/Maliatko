


using Microsoft.AspNetCore.Identity;

namespace BookStore_Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}
