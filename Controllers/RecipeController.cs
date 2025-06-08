using meal_menu_api.Database.Context;
using meal_menu_api.Dtos;
using meal_menu_api.Entities.Account;
using meal_menu_api.Entities.Recipes;
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
                            Id = ingredient.Id,
                            Description = ingredient.Description,
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
                            Id = step.Id,
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

        [HttpGet]
        [Route("get-recipe/{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetRecipe(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest();

            RecipeEntity? recipe = await _dataContext.Recipes.Include(recipe => recipe.Ingredients)
                .ThenInclude(ingredient => ingredient.Unit)
                .Include(recipe => recipe.Steps)
                .Include(recipe => recipe.Images)
                .FirstOrDefaultAsync(recipe => recipe.Id.ToString() == id);

            if (recipe != null)
            {
                RecipeDtoGet newRecipeDto = new()
                {
                    Id = recipe!.Id,
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
                        Id = ingredient.Id,
                        Description = ingredient.Description,
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
                        Id = step.Id,
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

                return Ok(newRecipeDto);
            }

            return NotFound();  
        }

        [HttpDelete]
        [Route("delete/{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> DeleteRecipe(string id)
        {
            RecipeEntity? recipeToDelete = await _dataContext.Recipes.FirstOrDefaultAsync(r => r.Id.ToString() == id) ?? null;

            if (recipeToDelete != null)
            {
                await _recipeManager.DeleteImage(recipeToDelete.Id);

                _dataContext.Recipes.Remove(recipeToDelete);
                await _dataContext.SaveChangesAsync();
                return NoContent();
            }

            return NotFound();
        }

        [HttpPut]
        [Route("update/{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> UpdateRecipe(string id, RecipeDtoCreate recipeDto )
        {
            AppUser? user = await _userManager.FindByEmailAsync(User!.Identity!.Name!);
            RecipeEntity? recipeEntity = await _dataContext.Recipes.FirstOrDefaultAsync(r => r.Id.ToString() == id) ?? null;
            List<IngredientDto> ingredientDtos = JsonConvert.DeserializeObject<List<IngredientDto>>(recipeDto.Ingredients!)!;
            List<StepDto> stepDtos = JsonConvert.DeserializeObject<List<StepDto>>(recipeDto.Steps!)!;

            recipeEntity!.Name = recipeDto.RecipeName;
            recipeEntity.Description = recipeDto.RecipeDescription; 
            recipeEntity.Ppl = recipeDto.Ppl;
            recipeEntity.UpdatedAt = DateTime.Now;

            if (stepDtos.Count < 1)
                await _dataContext.Steps.Where(s => s.RecipeId == recipeEntity.Id).ExecuteDeleteAsync();

            if (stepDtos.Count > 0)
            {
                List<StepDto> newSteps = [];
                List<StepDto> oldSteps = [];

                foreach (StepDto stepDto in stepDtos)
                {
                    if (stepDto.Id == -1)
                        newSteps.Add(stepDto);
                    else
                        oldSteps.Add(stepDto);
                }

                if (oldSteps.Count != 0)
                    await _recipeManager.UpdateStepsAsync(oldSteps, recipeEntity);

                if (newSteps.Count != 0)
                    await _recipeManager.SaveSteps(newSteps, recipeEntity);
            }

            if (ingredientDtos.Count < 1)         
                await _dataContext.Ingredients.Where(i => i.RecipeId == recipeEntity.Id).ExecuteDeleteAsync();
            
            if (ingredientDtos.Count > 0)
            {
                List<IngredientDto> newIngredients = [];
                List<IngredientDto> oldIngredients = [];

                foreach (IngredientDto ingredientDto in ingredientDtos)
                {
                    if (ingredientDto.Id == -1)
                        newIngredients.Add(ingredientDto);
                    else
                        oldIngredients.Add(ingredientDto);
                }

                if (oldIngredients.Count != 0)
                    await _recipeManager.UpdateIngredientsAsync(oldIngredients, recipeEntity);

                if (newIngredients.Count != 0)
                    await _recipeManager.SaveIngredients(newIngredients, recipeEntity);
            }

            if(recipeDto.DeleteImage && recipeDto.Image == null) //RADERA
                 await _recipeManager.DeleteImage(recipeEntity.Id);

            if(recipeDto.Image != null && !recipeDto.DeleteImage) //BYT UT //BEHÅLL
            {
                await _recipeManager.DeleteImage(recipeEntity.Id);
                await _recipeManager.SaveImages(recipeDto.Image, recipeEntity);
            }

            await _dataContext.SaveChangesAsync();

            return Ok();
        }
    }
}
