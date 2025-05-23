namespace meal_menu_api.Dtos
{
    public class RecipeDtoGet
    {
        public int Id { get; set; }
        
        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public int Ppl { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public List<IngredientDto> Ingredients { get; set; } = new();
        
        public List<StepDto> Steps { get; set; } = new();
        
        public List<ImageDto> Images { get; set; } = new();
    }
}
