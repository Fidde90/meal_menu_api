using meal_menu_api.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace meal_menu_api.Dtos
{
    public class IngredientDto
    {
        public string Name { get; set; } = null!;

        public int Amount { get; set; }

        public string Unit { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
