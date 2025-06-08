using meal_menu_api.Entities.Recipes;

namespace meal_menu_api.Managers
{
    public class DinnerScheduleManager
    {
        public void UpdateRotationPoints(List<RecipeEntity> recipes, int pointsToAdd = 25, int MaxPoints = 100, int resetPoints = 0)
        {
            foreach (var recipe in recipes)
            {
                recipe.RotationPoints += pointsToAdd;
                if (recipe.RotationPoints >= MaxPoints)
                    recipe.RotationPoints = resetPoints;
            }
        }

        public List<RecipeEntity> FillScheduleWithRecipes(List<RecipeEntity> selectedRecipes, List<RecipeEntity> shuffled, int days, int iterations)
        {
            List<RecipeEntity> returnList = [];
            Random rnd = new Random();  

            for (int i = 0; i < iterations; i++)
            {
                int recipe = rnd.Next(0, selectedRecipes.Count - 1);
                returnList.Add(selectedRecipes[recipe]);
            }

            return returnList;
        }
    }
}
