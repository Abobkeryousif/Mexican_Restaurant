using Microsoft.AspNetCore.Identity;

namespace Mexican_Restaurant.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Order>? Orders { get; set; }
    }
}
