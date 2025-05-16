
namespace meal_menu_api.Dtos
{
    public class ImageDto
    {
        public int Id { get; set; }

        public string ImageUrl { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
