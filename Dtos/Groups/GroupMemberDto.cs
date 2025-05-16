using meal_menu_api.Models.Enums;

namespace meal_menu_api.Dtos.Groups
{
    public class GroupMemberDto
    {
        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string UserName { get; set; } = null!;

        public string? Role { get; set; }

        public DateTime LastLogin { get; set; }

        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    }
}
