using meal_menu_api.Database.Context;
using meal_menu_api.Dtos;
using meal_menu_api.Entities;
using System.Diagnostics;

namespace meal_menu_api.Managers
{
    public class RecipeManager
    {
        private readonly DataContext _datacontext;

        public RecipeManager(DataContext datacontext)
        {
           _datacontext = datacontext; 
        }

        public async Task SaveIngredients(IEnumerable<IngredientDto> list, RecipeEntity recipe)
        {
            IEnumerable<UnitEntity> units = _datacontext.Units.ToList();
            var ingredientsToSave = new List<IngredientEntity>();

            foreach (var item in list)
            {
                UnitEntity unit = units.FirstOrDefault(x => x.Name == item.Unit)!;

                if (unit == null)
                    continue;

                IngredientEntity newIngredient = new IngredientEntity
                {
                    Name = item.Name,
                    Amount = item.Amount,
                    UnitId = unit.Id,
                    Unit = unit,
                    RecipeId = recipe.Id,
                    Recipe = recipe,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                ingredientsToSave.Add(newIngredient);
            }
            try
            {
                _datacontext.Ingredients.AddRange(ingredientsToSave);
                await _datacontext.SaveChangesAsync();
            }
            catch (Exception error)
            {
                Debug.WriteLine($"Error RecipeController, Saving Ingredients in SaveIngredients function: {error.Message}");
                Console.WriteLine($"Error RecipeController, Saving Ingredients in SaveIngredients function: {error.Message}");
                throw new Exception("Error saving Ingredients, transaction will be rolled back. ", error);
            }
        }

        public async Task SaveSteps(IEnumerable<StepDto> list, RecipeEntity recipe)
        {
            var StepsToSave = new List<StepEntity>();

            foreach (var item in list)
            {
                StepEntity newStep = new StepEntity
                {
                    Description = item.Description,
                    RecipeId = recipe.Id,
                    Recipe = recipe,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                StepsToSave.Add(newStep);
            }
            try
            {
                _datacontext.Steps.AddRange(StepsToSave);
                await _datacontext.SaveChangesAsync();
            }
            catch (Exception error)
            {
                Debug.WriteLine($"Error RecipeController, Saving Steps in SaveSteps function: {error.Message}");
                Console.WriteLine($"Error RecipeController, Saving Steps in SaveSteps function: {error.Message}");
                throw new Exception("Error saving Steps, transaction will be rolled back. ", error);
            }
        }

        public async Task SaveImages(IFormFile Image, RecipeEntity recipe)
        {
            var filePath = Path.Combine("Images", Image.FileName);
            using var stream = new FileStream(filePath, FileMode.Create);
            await Image.CopyToAsync(stream);

            try
            {
                ImageEntity newImage = new ImageEntity
                {
                    ImageUrl = filePath,
                    RecipeId = recipe.Id,
                    Recipe = recipe,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                await _datacontext.Images.AddAsync(newImage);
                await _datacontext.SaveChangesAsync();
            }
            catch (Exception error)
            {
                Debug.WriteLine($"Error RecipeController, Saving Image in SaveImages function: {error.Message}");
                Console.WriteLine($"Error RecipeController, Saving Image in SaveImages function: {error.Message}");
                throw new Exception("Error saving Images, transaction will be rolled back. ", error);
            }
        }
    }
}
