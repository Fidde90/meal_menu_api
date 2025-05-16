using System.ComponentModel.DataAnnotations;

namespace meal_menu_api.Models.Forms
{
    public class CreateGroupFormModel
    {
        [Required]
        [MinLength(1)]
        public string Name { get; set; } = null!;

        [MinLength(1)]
        public string? Description { get; set; }

        public IFormFile? Icon { get; set; }
    }
}
