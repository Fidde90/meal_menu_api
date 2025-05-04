using System.ComponentModel.DataAnnotations;

namespace meal_menu_api.Dtos
{
    public class RegisterUserDto
    {
        [Required(ErrorMessage = "First name is required!")]
        [MinLength(2, ErrorMessage = "Invalid first name!")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Last name is required!")]
        [MinLength(2, ErrorMessage = "Invalid last name!")]
        public string LastName { get; set; } = null!;

        [DataType(DataType.EmailAddress)]
        [RegularExpression("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email!")]
        [Required(ErrorMessage = "Email is required!")]
        public string Email { get; set; } = null!;

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required!")]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[\\W_]).{8,}$", ErrorMessage = "Invalid password!")]
        public string Password { get; set; } = null!;

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Required!")]
        [Compare(nameof(Password), ErrorMessage = "Passwords did not match!")]
        public string ConfirmPassword { get; set; } = null!;
    }
}