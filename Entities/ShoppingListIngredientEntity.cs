using System.ComponentModel.DataAnnotations.Schema;

namespace meal_menu_api.Entities
{
    public class ShoppingListIngredientEntity
    {
        public int Id { get; set; }

        [ForeignKey(nameof(ShoppingList))]
        public int ShoppingListId { get; set; }

        public ShoppingListEntity ShoppingList { get; set; } = null!;

        public string? Description { get; set; }

        public string Name { get; set; } = null!; 

        public double Amount { get; set; } 

        public string Unit { get; set; } = null!;

        public bool? IsChecked { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
