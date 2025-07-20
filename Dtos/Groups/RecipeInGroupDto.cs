namespace meal_menu_api.Dtos.Groups
{
    public class RecipeInGroupDto
    {
        public int GroupId { get; set; }

        public string GroupName { get; set; } = null!;

        public bool RecipeInGroup { get; set; } = false;
    }
}
