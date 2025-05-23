
using meal_menu_api.Models.Enums;

namespace meal_menu_api.Entities
{
    public class GroupMemberEntity
    {
        public int GroupId { get; set; }
        public GroupEntity Group { get; set; } = default!;

        public string UserId { get; set; } = null!;

        public AppUser User { get; set; } = default!;

        public GroupRole Role { get; set; }

        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    }
}
