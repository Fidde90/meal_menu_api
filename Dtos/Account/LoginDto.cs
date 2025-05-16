using System.ComponentModel.DataAnnotations;

namespace meal_menu_api.Dtos.Account
{
    public class LoginDto
    {
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Email is required!")]
        [RegularExpression("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email!")]
        public string Email { get; set; } = null!;

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required!")]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[\\W_]).{8,}$", ErrorMessage = "Invalid password!")]
        public string Password { get; set; } = null!;
    }
}
