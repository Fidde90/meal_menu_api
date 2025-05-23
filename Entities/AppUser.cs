using Microsoft.AspNetCore.Identity;

namespace meal_menu_api.Entities
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public DateTime LastLogin { get; set; }

        public ICollection<GroupMemberEntity> GroupMemberships { get; set; } = [];

        public List<ShoppingListEntity> ShoppingLists { get; set; } = [];

        public List<DinnerScheduleEntity> DinnerSchedules { get; set; } = [];

        public List<RecipeEntity> Recipes { get; set; } = [];
    }
}
