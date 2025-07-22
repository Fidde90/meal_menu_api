using meal_menu_api.Entities.Dinners;
using meal_menu_api.Entities.Recipes;
using meal_menu_api.Entities.ShoppingList;
using Microsoft.AspNetCore.Identity;

namespace meal_menu_api.Entities.Account
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public DateTime LastLogin { get; set; }

        public ICollection<GroupMemberEntity> GroupMemberships { get; set; } = [];

        public List<ShoppingListEntity> ShoppingLists { get; set; } = [];

        public List<DinnerScheduleEntity> DinnerSchedules { get; set; } = [];

        public List<RecipeEntity> Recipes { get; set; } = [];
    }
}
