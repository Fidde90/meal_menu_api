using meal_menu_api.Models.Enums;

namespace meal_menu_api.Dtos.Groups
{
    public class GroupInvitationDto
    {
        public InvitedByUserDto InvitedByUser { get; set; } = null!;

        public InvitedUserDto InvitedUser { get; set; } = null!;

        public string? Message { get; set; }

        public DateTime SentAt { get; set; } = DateTime.Now;

        public InvitationStatus Status { get; set; } = InvitationStatus.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
