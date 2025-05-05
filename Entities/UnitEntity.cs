using System.ComponentModel.DataAnnotations;

namespace meal_menu_api.Entities
{
    public class UnitEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(1)]
        public string Name { get; set; } = null!;
    }
}
