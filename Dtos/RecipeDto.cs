namespace meal_menu_api.Dtos
{
    public class RecipeDto
    {
        public string RecipeName { get; set; } = null!;

        public string RecipeDescription { get; set; } = null!;

        public int Ppl {  get; set; }

        public IFormFile? Image { get; set; }

        public string? Ingredients { get; set; }

        public string? Steps { get; set; }
    }
}
