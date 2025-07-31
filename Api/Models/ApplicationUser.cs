using Microsoft.AspNetCore.Identity;

namespace Api.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string Firstname { get; set; } = string.Empty;
        public string Lastname { get; set; } = string.Empty;
        
        public string Country { get; set; } = string.Empty;
        public string State { get; set; }  = string.Empty;

        public override string? NormalizedUserName => Firstname +" "+ Lastname;
    }

    public class ApplicationRole : IdentityRole<Guid>{}
}
