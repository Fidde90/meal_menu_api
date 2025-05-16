using meal_menu_api.Entities;

namespace meal_menu_api.Dtos
{
    public class DinnerScheduleDto
    {
        public DateTime StartsAtDate { get; set; }

        public DateTime EndsAtDate { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public List<DinnerDto> Dinners { get; set; } = new List<DinnerDto>();
    }
}
