
namespace meal_menu_api.Dtos
{
    public class StepDto
    {
        public int Id { get; set; }

        public string Description { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
