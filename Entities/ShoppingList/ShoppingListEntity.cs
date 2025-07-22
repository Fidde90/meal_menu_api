using meal_menu_api.Entities.Account;
using meal_menu_api.Entities.Dinners;
using meal_menu_api.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace meal_menu_api.Entities.ShoppingList
{
    public class ShoppingListEntity
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; } = null!;

        public AppUser User { get; set; } = null!;

        [ForeignKey(nameof(DinnerSchedule))]
        public int DinnerScheduleId { get; set; }

        public DinnerScheduleEntity DinnerSchedule { get; set; } = null!;

        public string? Name { get; set; }

        public string? Notes { get; set; }

        public ShoppingListStatus Status { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public List<ShoppingListIngredientEntity> Ingredients { get; set; } = [];
    }
}
