using meal_menu_api.Database.Context;
using meal_menu_api.Dtos;
using meal_menu_api.Entities;
using meal_menu_api.Entities.Recipes;
using meal_menu_api.Mappers;
using Microsoft.EntityFrameworkCore;
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

                ingredientsToSave.Add(IngredientMapper.ToIngredientEntity(item, recipe, unit));
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

        public async Task UpdateIngredientsAsync(List<IngredientDto> ingredientDtoList, RecipeEntity recipe)
        {
            List<IngredientEntity> ingredientEntities = _datacontext.Ingredients.Where(i => i.RecipeId == recipe.Id).ToList();

            if (ingredientEntities == null)
                return;

            var ingredientsToRemove = ingredientEntities
                .Where(entity => !ingredientDtoList.Any(dto => dto.Id == entity.Id))
                .ToList();

            _datacontext.Ingredients.RemoveRange(ingredientsToRemove);

            List<IngredientEntity> UpdatedEntities = [];
            Dictionary<int, IngredientDto> dtoDictionary = ingredientDtoList.ToDictionary(dto => dto.Id);
            List<UnitEntity> units = await _datacontext.Units.ToListAsync();
            UnitEntity? unit;

            foreach (IngredientEntity ingredient in ingredientEntities)
            {
                if (dtoDictionary.TryGetValue(ingredient.Id, out var dto))
                {
                    ingredient.Description = dto.Description;
                    ingredient.Name = dto.Name;
                    ingredient.Amount = dto.Amount;

                    if (!string.IsNullOrEmpty(dto.Unit))
                    {
                        unit = units.FirstOrDefault(u => u.Name == dto.Unit)!;
                        ingredient.UnitId = unit.Id;
                        ingredient.Unit = unit;
                    }

                    ingredient.UpdatedAt = DateTime.Now;
                }
            }

            await _datacontext.SaveChangesAsync();
        }

        public async Task UpdateStepsAsync(List<StepDto> stepDtoList, RecipeEntity recipe)
        {
            List<StepEntity> stepEntities = _datacontext.Steps.Where(s => s.RecipeId == recipe.Id).ToList();

            if (stepEntities == null)
                return;

            var StepsToRemove = stepEntities
                .Where(entity => !stepDtoList.Any(dto => dto.Id == entity.Id))
                .ToList();

            _datacontext.Steps.RemoveRange(StepsToRemove);

            List<StepEntity> UpdatedEntities = [];
            Dictionary<int, StepDto> dtoDictionary = stepDtoList.ToDictionary(dto => dto.Id);

            foreach (StepEntity step in stepEntities)
            {
                if (dtoDictionary.TryGetValue(step.Id, out var dto))
                {
                    step.Description = dto.Description;
                    step.UpdatedAt = DateTime.Now;
                }
            }

            await _datacontext.SaveChangesAsync();
        }

        public async Task SaveSteps(IEnumerable<StepDto> list, RecipeEntity recipe)
        {
            var StepsToSave = new List<StepEntity>();

            foreach (var item in list)
                StepsToSave.Add(StepMapper.ToStepEntity(item,recipe));

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
            string filePath = Path.Combine("Images", $"{recipe.Id}_" + Image.FileName);
            using var stream = new FileStream(filePath, FileMode.Create);
            await Image.CopyToAsync(stream);

            try
            {
                await _datacontext.Images.AddAsync(ImageMapper.ToImageEntity(filePath, recipe));
                await _datacontext.SaveChangesAsync();
            }
            catch (Exception error)
            {
                Debug.WriteLine($"Error RecipeController, Saving Image in SaveImages function: {error.Message}");
                Console.WriteLine($"Error RecipeController, Saving Image in SaveImages function: {error.Message}");
                throw new Exception("Error saving Images, transaction will be rolled back. ", error);
            }
        }

        public async Task DeleteImage(int recipeId)
        {
            ImageEntity? existingImage = await _datacontext.Images.FirstOrDefaultAsync(i => i.RecipeId == recipeId);
            if(existingImage != null)
            {
                var filePath = Path.Combine(existingImage!.ImageUrl);
                File.Delete(filePath);

                _datacontext.Images.Remove(existingImage);
                await _datacontext.SaveChangesAsync();
            }
        }
    }
}
