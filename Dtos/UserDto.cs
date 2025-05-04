namespace meal_menu_api.Dtos
{
    public class UserDto
    {
        public string? FirstName { get; set; } 
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public string? PhoneNumber { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool TowFactorEnabeld { get; set; }
        public DateTime LastLogin {  get; set; } 
    }
}
