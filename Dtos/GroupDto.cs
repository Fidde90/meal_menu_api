using meal_menu_api.Entities;

namespace meal_menu_api.Dtos
{
    public class GroupDto
    {
        public int Id { get; set; }

        public string OwnerId { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public string? IconUrl { get; set; }

        public ICollection<GroupMemberEntity> Members { get; set; } = [];

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
