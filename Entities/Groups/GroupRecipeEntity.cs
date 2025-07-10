using meal_menu_api.Entities.Account;
using meal_menu_api.Entities.Recipes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace meal_menu_api.Entities.Groups
{
    public class GroupRecipeEntity
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Group))]
        public int GroupId { get; set; }

        public GroupEntity Group { get; set; } = null!;

        [ForeignKey(nameof(SharedByUser))]
        public string SharedByUserId { get; set; } = null!;

        public AppUser SharedByUser { get; set; } = null!;

        [ForeignKey(nameof(Recipe))]
        public int RecipeId { get; set; }

        public RecipeEntity Recipe { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}