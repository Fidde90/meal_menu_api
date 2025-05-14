using System.ComponentModel.DataAnnotations.Schema;

namespace meal_menu_api.Entities
{
    public class DinnerEntity
    {
        public int Id { get; set; }

        [ForeignKey(nameof(DinnerSchedule))]
        public int DinnerScheduleId { get; set; }
        public DinnerScheduleEntity DinnerSchedule { get; set; } = null!;
      
        //[ForeignKey(nameof(Recipe))]
        // public int? RecipeId { get; set; }
     
        //public RecipeEntity? Recipe { get; set; }

        public DateTime EatAt { get; set; }
        
        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
