using meal_menu_api.Dtos;
using meal_menu_api.Entities.Account;
using meal_menu_api.Entities.Recipes;

namespace meal_menu_api.Mappers
{
    public static class RecipeMapper
    {
        public static RecipeEntity ToRecipeEntity(RecipeDtoCreate recipeDto, AppUser user)
        {
            RecipeEntity newRecipe = new RecipeEntity
            {
                Name = recipeDto.RecipeName!,
                Description = recipeDto.RecipeDescription!,
                Ppl = recipeDto.Ppl,
                UserId = user.Id,
                User = user,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            return newRecipe;
        }
        public static RecipeDtoGet ToRecipeDtoGet(RecipeEntity recipe)
        {
            RecipeDtoGet newRecipeDto = new()
            {
                Id = recipe.Id,
                Name = recipe.Name,
                Description = recipe.Description,
                Ppl = recipe.Ppl,
                CreatedAt = recipe.CreatedAt,
                UpdatedAt = recipe.UpdatedAt,
            };

            return newRecipeDto;
        }
    }
}
