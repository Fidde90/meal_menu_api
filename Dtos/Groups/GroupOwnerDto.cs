using meal_menu_api.Models.Enums;

namespace meal_menu_api.Dtos.Groups
{
    public class GroupOwnerDto
    {
        public string? FirstName { get; set; } 

        public string? LastName { get; set; } 

        public string? Email { get; set; }

        public string? UserName { get; set; } = null!;

        public string? Role { get; set; }

        public DateTime? LastLogin { get; set; }
    }
}
