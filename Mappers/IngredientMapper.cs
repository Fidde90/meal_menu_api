using meal_menu_api.Dtos;
using meal_menu_api.Entities;
using meal_menu_api.Entities.Recipes;

namespace meal_menu_api.Mappers
{
    public static class IngredientMapper
    {
        public static List<IngredientDto> IngredientsToDtos(List<IngredientEntity> entites)
        {
            if (entites.Count <= 0)
                return [];

            List<IngredientDto> listToReturn = [];

            foreach (var entity in entites)
            {
                IngredientDto newDto = new IngredientDto
                {
                    Name = entity.Name,
                    Amount = entity.Amount,
                    Unit = entity.Unit.Name,
                    CreatedAt = entity.CreatedAt,
                    UpdatedAt = entity.UpdatedAt,
                };

                listToReturn.Add(newDto);
            }

            return listToReturn;
        }

        public static IngredientEntity ToIngredientEntity (IngredientDto dto, RecipeEntity recipe, UnitEntity unit)
        {
            IngredientEntity newIngredient = new() 
            { 
                Description = dto.Description,
                Name = dto.Name,
                Amount = dto.Amount,
                UnitId = unit.Id,
                Unit = unit,
                RecipeId = recipe.Id,
                Recipe = recipe,
            };

            return newIngredient;
        }
    }
}
