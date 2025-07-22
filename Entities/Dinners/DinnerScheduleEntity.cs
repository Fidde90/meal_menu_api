using meal_menu_api.Entities.Account;
using meal_menu_api.Entities.ShoppingList;
using System.ComponentModel.DataAnnotations.Schema;

namespace meal_menu_api.Entities.Dinners
{
    public class DinnerScheduleEntity
    {
        public int Id { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; } = null!;

        public AppUser User { get; set; } = null!;

        public DateTime StartsAtDate { get; set; }

        public DateTime EndsAtDate { get; set; }
        
        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public ShoppingListEntity? ShoppingList { get; set; }

        public List<DinnerEntity> Dinners { get; set; } = new List<DinnerEntity>();
    }
}
