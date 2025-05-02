using Microsoft.AspNetCore.Identity;

namespace meal_menu_api.Entities
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

    }
}
