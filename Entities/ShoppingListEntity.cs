using meal_menu_api.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace meal_menu_api.Entities
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

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public List<ShoppingListIngredientEntity> Ingredients { get; set; } = new List<ShoppingListIngredientEntity>();
    }
}
