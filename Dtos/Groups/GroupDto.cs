
namespace meal_menu_api.Dtos.Groups
{
    public class GroupDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public string? IconUrl { get; set; }

        public List<GroupMemberDto> Members { get; set; } = [];

        public List<GroupInvitationDto> Invites { get; set; } = [];

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
