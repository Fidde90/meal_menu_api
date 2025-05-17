using meal_menu_api.Models.Enums;

namespace meal_menu_api.Dtos.Groups
{
    public class InvitedUserDto
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string UserName { get; set; } = null!;

        public string status  = InvitationStatus.Pending.ToString();
    }
}
