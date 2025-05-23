using System.ComponentModel.DataAnnotations;

namespace meal_menu_api.Models.Forms
{
    public class DinnerScheduleFormModel
    {
        [Required]
        public int NumberOfDays { get; set; }
    }
}
