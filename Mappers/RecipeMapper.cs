using meal_menu_api.Dtos;
using meal_menu_api.Dtos.Groups;
using meal_menu_api.Entities.Account;
using meal_menu_api.Entities.Groups;
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

        public static GroupRecipeDto MapFullGroupRecipeDto(GroupRecipeEntity groupRecipeEntity)
        {
            if (groupRecipeEntity == null)
                return null!;

            var newGroupRecipeDto = new GroupRecipeDto
            {
                Id = groupRecipeEntity.Id,
                SharedByName = groupRecipeEntity.SharedBy.FirstName + " " + groupRecipeEntity.SharedBy.LastName,
                RecipeId = groupRecipeEntity.RecipeId,
                Name = groupRecipeEntity.Name,
                Description = groupRecipeEntity.Description,
                Ppl = groupRecipeEntity.Ppl,
                CreatedAt= groupRecipeEntity.CreatedAt,
                UpdatedAt= groupRecipeEntity.UpdatedAt,
                Images = ImageMapper.ImagesToDtos(groupRecipeEntity.Images) ?? [],
                Ingredients = IngredientMapper.IngredientsToDtos(groupRecipeEntity.Ingredients) ?? [],
                Steps = StepMapper.StepsToDtos(groupRecipeEntity.Steps) ?? []   
            };

            return newGroupRecipeDto;
        }

        public static List<GroupRecipeDto> MapFullGroupRecipeDtos(List<GroupRecipeEntity> entityList)
        {
            if (entityList.Count < 1)
                return [];

            List<GroupRecipeDto> returnList = [];

            foreach (var entity in entityList)
            {
                var newDto = RecipeMapper.MapFullGroupRecipeDto(entity);
                returnList.Add(newDto);
            }

            return returnList;
        }
    }
}
