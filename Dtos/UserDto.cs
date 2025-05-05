namespace meal_menu_api.Dtos
{
    public class UserDto
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool TowFactorEnabeld { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime LastLogin {  get; set; } 
    }
}
