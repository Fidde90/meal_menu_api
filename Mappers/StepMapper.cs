using meal_menu_api.Dtos;
using meal_menu_api.Entities.Recipes;

namespace meal_menu_api.Mappers
{
    public static class StepMapper
    {
        public static StepEntity ToStepEntity(StepDto dto, RecipeEntity recipe)
        {
            StepEntity newStep = new StepEntity
            {
                Description = dto!.Description,
                RecipeId = recipe.Id,
                Recipe = recipe,
            };

            return newStep;
        }

        public static List<StepDto> StepsToDtos(List<StepEntity> entites)
        {
            if (entites.Count <= 0)
                return [];

            List<StepDto> listToReturn = [];

            foreach (var entity in entites)
            {
                StepDto newDto = new StepDto
                {
                    Id = entity.Id,
                    Description = entity.Description,
                    CreatedAt = entity.CreatedAt,
                    UpdatedAt = entity.UpdatedAt
                };

                listToReturn.Add(newDto);
            }

            return listToReturn;
        }
    }
}
