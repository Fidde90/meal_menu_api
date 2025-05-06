using meal_menu_api.Database.Context;
using meal_menu_api.Dtos;
using meal_menu_api.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System.Diagnostics;

namespace meal_menu_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecipeController : ControllerBase
    {

        private readonly DataContext _datacontext;

        public RecipeController(DataContext datacontext)
        {
            _datacontext = datacontext;
        }

        [HttpPost]
        [Route("create")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> CreateRecipe(RecipeDto recipeDto)
        {

            //LÄGG TILL ATT EN USER KAN HA MÅNGA RECEPT!!!
            if (!ModelState.IsValid)
                return BadRequest();

            var Ingredients = JsonConvert.DeserializeObject<IEnumerable<IngredientDto>>(recipeDto.Ingredients!);
            var Steps = JsonConvert.DeserializeObject<IEnumerable<StepDto>>(recipeDto.Steps!);

            using var transaction = await _datacontext.Database.BeginTransactionAsync();

            try
            {
                RecipeEntity newRecipe = new RecipeEntity
                {
                    Name = recipeDto.RecipeName!,
                    Description = recipeDto.RecipeDescription!,
                    Ppl = recipeDto.Ppl
                };

                _datacontext.Recipes.Add(newRecipe);
                await _datacontext.SaveChangesAsync();

                await SaveIngredients(Ingredients!, newRecipe);
                await SaveSteps(Steps!, newRecipe);
                await SaveImages(recipeDto.Image!, newRecipe);

                await transaction.CommitAsync();
            }
            catch (Exception error)
            {
                await transaction.RollbackAsync();
                Debug.WriteLine($"Error RecipeController, Saving Ingredients in SaveIngredients function: {error.Message}");
                Console.WriteLine($"Error RecipeController, Saving Ingredients in SaveIngredients function: {error.Message}");
                throw new Exception("An error occurred while saving the recipe.", error);
            }

            return Ok();
        }

        private async Task SaveIngredients(IEnumerable<IngredientDto> list, RecipeEntity recipe)
        {
            IEnumerable<UnitEntity> units = _datacontext.Units.ToList();
            var ingredientsToSave = new List<IngredientEntity>();

            foreach (var item in list)
            {
                UnitEntity unit = units.FirstOrDefault(x => x.Name == item.Unit)!;

                IngredientEntity newIngredient = new IngredientEntity
                {
                    Name = item.Name,
                    Amount = item.Amount,
                    UnitId = unit.Id,
                    Unit = unit,
                    RecipeId = recipe.Id,
                    Recipe = recipe,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
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
        private async Task SaveSteps(IEnumerable<StepDto> list, RecipeEntity recipe)
        {
            var StepsToSave = new List<StepEntity>();

            foreach (var item in list)
            {
                StepEntity newStep = new StepEntity
                {
                    Description = item.Description,
                    RecipeId = recipe.Id,
                    Recipe = recipe,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
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

        private async Task SaveImages(IFormFile Image, RecipeEntity recipe)
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
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
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
