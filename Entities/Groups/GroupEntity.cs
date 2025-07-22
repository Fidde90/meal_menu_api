using meal_menu_api.Entities.Groups;
using System.ComponentModel.DataAnnotations;

namespace meal_menu_api.Entities
{
    public class GroupEntity
    {
        [Key]
        public int Id { get; set; }

        public string OwnerId { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public string? IconUrl { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public ICollection<GroupMemberEntity> Members { get; set; } = [];

        public ICollection<GroupInvitationEntity> Invitations { get; set; } = [];

        public ICollection<GroupRecipeEntity> GroupRecipes { get; set; } = [];
    }
}
