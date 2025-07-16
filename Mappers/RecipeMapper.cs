using meal_menu_api.Dtos;
using meal_menu_api.Dtos.Groups;
using meal_menu_api.Entities;
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
            if (recipe == null)
                return null!;

            RecipeDtoGet newRecipeDto = new()
            {
                Id = recipe.Id,
                Name = recipe.Name,
                Description = recipe.Description,
                Ppl = recipe.Ppl,
                CreatedAt = recipe.CreatedAt,
                UpdatedAt = recipe.UpdatedAt,
                Ingredients = IngredientMapper.IngredientsToDtos(recipe.Ingredients),
                Steps = StepMapper.StepsToDtos(recipe.Steps),
                Images = ImageMapper.ImagesToDtos(recipe.Images)
            };

            return newRecipeDto;
        }

        public static GroupRecipeEntity ToGroupRecipeEntity(GroupEntity group, RecipeEntity recipe, AppUser user)
        {
            if (group == null || recipe == null || user == null)
                return null!;

            var newGroupRecipeEntiy = new GroupRecipeEntity
            {
                GroupId = group.Id,
                Group = group,
                SharedByUserId = user.Id,
                SharedByUser = user,
                RecipeId = recipe.Id,
                Recipe = recipe
            };

            return newGroupRecipeEntiy;
        }

        public static GroupRecipeDto ToGroupRecipeDto(GroupRecipeEntity groupRecipeEntity)
        {
            if (groupRecipeEntity == null)
                return null!;

            var newGroupRecipeDto = new GroupRecipeDto
            {
                Id = groupRecipeEntity.Id,
                SharedByName = groupRecipeEntity.SharedByUser.FirstName + " " + groupRecipeEntity.SharedByUser.LastName,
                RecipeDto = RecipeMapper.ToRecipeDtoGet(groupRecipeEntity.Recipe),
                CreatedAt = groupRecipeEntity.CreatedAt,
                UpdatedAt = groupRecipeEntity.UpdatedAt,
            };

            return newGroupRecipeDto;
        }

        public static List<GroupRecipeDto> ToGroupRecipeDtos(List<GroupRecipeEntity> entityList)
        {
            if (entityList.Count < 1)
                return [];

            List<GroupRecipeDto> returnList = [];

            foreach (var entity in entityList)
            {
                var newDto = RecipeMapper.ToGroupRecipeDto(entity);
                returnList.Add(newDto);
            }

            return returnList;
        }
    }
}
