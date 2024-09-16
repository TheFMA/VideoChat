using Microsoft.AspNetCore.Identity;

namespace Video_Chat.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }
    }
}
