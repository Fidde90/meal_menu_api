using meal_menu_api.Dtos;
using meal_menu_api.Entities.Recipes;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace meal_menu_api.Mappers
{
    public static class IngredientMapper
    {
        public static List<IngredientDto> MapManyIngredientsToDto(List<IngredientEntity> entites)
        {
            if (entites.Count <= 0)
                return null!;

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
    }
}
