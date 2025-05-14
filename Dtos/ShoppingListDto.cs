using meal_menu_api.Entities;
using meal_menu_api.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace meal_menu_api.Dtos
{
    public class ShoppingListDto
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Notes { get; set; }

        public ShoppingListStatus Status { get; set; }

        public List<ShoppingListIngredientDto> Ingredients { get; set; } = new();

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
