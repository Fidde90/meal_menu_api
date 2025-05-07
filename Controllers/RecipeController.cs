using meal_menu_api.Database.Context;
using meal_menu_api.Dtos;
using meal_menu_api.Entities;
using meal_menu_api.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;

namespace meal_menu_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecipeController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly RecipeManager _recipeManager;
        private readonly UserManager<AppUser> _userManager;

        public RecipeController(DataContext dataContext, RecipeManager recipeManager, UserManager<AppUser> userManager)
        {
            _dataContext = dataContext;
            _recipeManager = recipeManager;
            _userManager = userManager;
        }

        [HttpPost]
        [Route("create")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> CreateRecipe(RecipeDtoCreate recipeDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

   
            AppUser? user = await _userManager.FindByEmailAsync(User.Identity!.Name!);

            if (user == null)
                return Unauthorized();

            using var transaction = await _dataContext.Database.BeginTransactionAsync();

            try
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

                _dataContext.Recipes.Add(newRecipe);
                await _dataContext.SaveChangesAsync();

                if (!string.IsNullOrEmpty(recipeDto.Ingredients) && !string.IsNullOrEmpty(recipeDto.Steps))
                {
                    IEnumerable<IngredientDto> ingredients = [];
                    IEnumerable<StepDto> steps = [];

                    ingredients = JsonConvert.DeserializeObject<IEnumerable<IngredientDto>>(recipeDto.Ingredients!)!;
                    steps = JsonConvert.DeserializeObject<IEnumerable<StepDto>>(recipeDto.Steps!)!;

                    if(ingredients != null && ingredients.Any())
                        await _recipeManager.SaveIngredients(ingredients!, newRecipe);

                    if(steps != null && steps.Any())
                        await _recipeManager.SaveSteps(steps!, newRecipe);
                }

                if (recipeDto.Image != null)
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

        [HttpGet]
        [Route("get-all")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetAllRecipes()
        {
            AppUser? user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                List<RecipeEntity> recipeEntities = _dataContext.Recipes.Where(r => r.UserId == user.Id)
                    .Include(r => r.Ingredients).ThenInclude(i => i.Unit)
                    .Include(r => r.Steps)
                    .Include(r => r.Images)
                    .OrderByDescending(r => r.CreatedAt)
                    .AsNoTracking()
                    .ToList();

                if (recipeEntities.Count < 1)
                    return NotFound();

                List<RecipeDtoGet> recipeDtos = [];

                foreach(RecipeEntity recipe in recipeEntities)
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

                    foreach (IngredientEntity ingredient in recipe.Ingredients)
                    {
                        IngredientDto newIngredient = new()
                        {
                            Name = ingredient.Name,
                            Amount = ingredient.Amount,
                            Unit = ingredient.Unit.Name ?? null,
                            CreatedAt = ingredient.CreatedAt,
                            UpdatedAt = ingredient.UpdatedAt,
                        };

                        newRecipeDto.Ingredients.Add(newIngredient);
                    }

                    foreach (StepEntity step in recipe.Steps)
                    {
                        StepDto newStep = new()
                        {
                            Description = step.Description,
                            CreatedAt = step.CreatedAt,
                            UpdatedAt = step.UpdatedAt,
                        };

                        newRecipeDto.Steps.Add(newStep);
                    }

                    foreach (ImageEntity image in recipe.Images)
                    {
                        ImageDto newImage = new()
                        {
                            Id = image.Id,
                            ImageUrl = image.ImageUrl?.Replace("\\", "/")!,
                            CreatedAt = image.CreatedAt,
                            UpdatedAt = image.UpdatedAt,
                        };

                        newRecipeDto.Images.Add(newImage);
                    }

                    recipeDtos.Add(newRecipeDto);

                }

                return Ok(recipeDtos);
            }

            return Unauthorized();
        }
    }
}
