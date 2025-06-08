using meal_menu_api.Dtos;
using meal_menu_api.Entities.Recipes;

namespace meal_menu_api.Mappers
{
    public static class ImageMapper
    {
        public static ImageEntity ToImageEntity(string filePath, RecipeEntity recipe)
        {
            ImageEntity newImage = new ImageEntity
            {
                ImageUrl = filePath,
                RecipeId = recipe.Id,
                Recipe = recipe,
            };

            return newImage;
        }

        public static List<ImageDto> ImagesToDtos(List<ImageEntity> entites)
        {
            if (entites.Count <= 0)
                return [];

            List<ImageDto> listToReturn = [];

            foreach (var entity in entites)
            {
                ImageDto newImage = new()
                {
                    Id = entity.Id,
                    ImageUrl = entity.ImageUrl?.Replace("\\", "/")!,
                    CreatedAt = entity.CreatedAt,
                    UpdatedAt = entity.UpdatedAt,
                };

                listToReturn.Add(newImage);
            }

            return listToReturn;
        }
    }
}
