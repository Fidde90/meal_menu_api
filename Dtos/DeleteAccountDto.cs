using System.ComponentModel.DataAnnotations;

namespace meal_menu_api.Dtos
{
    public class DeleteAccountDto
    {
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required!")]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[\\W_]).{8,}$", ErrorMessage = "Invalid password!")]
        public string Password { get; set; } = null!;
    }
}
