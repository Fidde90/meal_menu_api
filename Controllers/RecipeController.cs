using meal_menu_api.Database.Context;
using meal_menu_api.Dtos;
using meal_menu_api.Entities;
using meal_menu_api.Managers;
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
        private readonly RecipeManager _recipeManager;

        public RecipeController(DataContext datacontext, RecipeManager recipeManager)
        {
            _datacontext = datacontext;
            _recipeManager = recipeManager;
        }

        [HttpPost]
        [Route("create")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> CreateRecipe(RecipeDto recipeDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            IEnumerable<IngredientDto> Ingredients = [];
            IEnumerable<StepDto> Steps = [];

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

                if (!string.IsNullOrEmpty(recipeDto.Ingredients) && !string.IsNullOrEmpty(recipeDto.Steps))
                {
                    Ingredients = JsonConvert.DeserializeObject<IEnumerable<IngredientDto>>(recipeDto.Ingredients!)!;
                    Steps = JsonConvert.DeserializeObject<IEnumerable<StepDto>>(recipeDto.Steps!)!;

                    await _recipeManager.SaveIngredients(Ingredients!, newRecipe);
                    await _recipeManager.SaveSteps(Steps!, newRecipe);
                }

                if(recipeDto.Image != null)        
                    await _recipeManager.SaveImages(recipeDto.Image!, newRecipe);

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
    }
}
