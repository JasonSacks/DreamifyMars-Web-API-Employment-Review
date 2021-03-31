using Microsoft.AspNetCore.Identity;

namespace DreamInMars.Model
{
    public class DreamUser : IdentityUser
    {
        public int AccountId { get; set; }
    }
}
