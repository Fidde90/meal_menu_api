namespace meal_menu_api.Dtos
{
    public class ShoppingListIngredientDto
    {
        public int Id { get; set; }

        public int ShoppingListId { get; set; }

        public string? Description { get; set; }

        public string Name { get; set; } = null!;

        public double Amount { get; set; }

        public string Unit { get; set; } = null!;

        public bool? IsChecked { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
