using System.ComponentModel.DataAnnotations;

namespace meal_menu_api.Models.Forms
{
    public class UpdateGroupFormModel
    {
        public string GroupId { get; set; } = null!;

        [Required]
        [MinLength(1)]
        public string Name { get; set; } = null!;

        [MinLength(1)]
        public string? Description { get; set; }

        public bool DeleteIcon { get; set; }

        public IFormFile? Icon { get; set; }
    }
}
