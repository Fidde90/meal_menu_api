using meal_menu_api.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace meal_menu_api.Entities
{
    public class GroupInvitationEntity
    {
        [Key]
        public int Id { get; set; }

        public string InvitedByUserId { get; set; } = null!;
        public AppUser InvitedByUser { get; set; } = null!;

        public string InvitedUserId { get; set; } = null!;
        public AppUser InvitedUser { get; set; } = null!;

        public int GroupId { get; set; }
        public GroupEntity Group { get; set; } = null!;

        public string? Message { get; set; }

        public DateTime SentAt { get; set; } = DateTime.Now;

        public InvitationStatus Status { get; set; } = InvitationStatus.Pending;

        public DateTime? RespondedAt { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
