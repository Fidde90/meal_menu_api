
namespace meal_menu_api.Dtos
{
    public class DinnerDto
    {   
        public int Id { get; set; }

        public int RecipeId { get; set; }
        
        public string? Name { get; set; }

        public string? Description { get; set; } 

        public int Ppl { get; set; }

        public string? ImageUrl { get; set; } 

        public DateTime EatAt { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
