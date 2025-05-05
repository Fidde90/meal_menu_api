using System.ComponentModel.DataAnnotations;

namespace meal_menu_api.Dtos
{
    public class UpdateUserDto
    {
        [MinLength(2)]
        public string FirstName { get; set; } = null!;

        [MinLength(2)]
        public string LastName { get; set; } = null!;

        [DataType(DataType.EmailAddress)]
        [RegularExpression("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email!")]
        public string Email { get; set; } = null!;

        public string? UserName { get; set; }

        public string? PhoneNumber { get; set; }

        public bool EmailConfirmed { get; set; }

        public bool TowFactorEnabeld { get; set; }

        public DateTime Updated { get; set; }
    }
}
