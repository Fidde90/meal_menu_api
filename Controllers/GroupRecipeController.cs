using meal_menu_api.Database.Context;
using meal_menu_api.Dtos;
using meal_menu_api.Dtos.Groups;
using meal_menu_api.Entities;
using meal_menu_api.Entities.Account;
using meal_menu_api.Entities.Groups;
using meal_menu_api.Entities.Recipes;
using meal_menu_api.Filters;
using meal_menu_api.Mappers;
using meal_menu_api.Models.Forms;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace meal_menu_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [UseApiKey]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class GroupRecipeController : ControllerBase
    {
        private readonly DataContext _dataContext;
        public GroupRecipeController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpPost]
        public async Task<IActionResult> CreateGroupRecipe(CreateGroupRecipeModel requestModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            AppUser? user = await _dataContext.Users.FirstOrDefaultAsync(u => u.Email == User.Identity!.Name);

            if (user == null)
                return Unauthorized();

            List<GroupEntity> groups = await _dataContext.Groups
                                                            .Include(g => g.Members.Where(m => m.UserId == user.Id))
                                                            .Where(g => requestModel.GroupIds.Contains(g.Id))
                                                            .ToListAsync();

            if (!groups.Any())
                return NotFound("no groups found");

            RecipeEntity? recipe = await _dataContext.Recipes
                                                        .Include(r => r.Ingredients)
                                                        .Include(r => r.Images)
                                                        .Include(r => r.Steps)
                                                        .FirstOrDefaultAsync(r => r.Id == requestModel.RecipeId);

            if (recipe == null)
                return NotFound("recipe not found");

            foreach (GroupEntity group in groups)
            {
                var groupRecipes = await _dataContext.GroupRecipes.Where(gr => gr.GroupId == group.Id).ToListAsync();
                var alreadyInGroup = groupRecipes.FirstOrDefault(gr => gr.RecipeId == recipe.Id);

                if (alreadyInGroup != null)
                    continue;

                var newGroupRecipeEntity = RecipeMapper.ToGroupRecipeEntity(group, recipe, user);
                await _dataContext.GroupRecipes.AddAsync(newGroupRecipeEntity);
            }

            await _dataContext.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAllGroupRecipes(int id)
        {

            List<GroupRecipeEntity> groupRecipes = await _dataContext.GroupRecipes
                                                            .Include(gr => gr.Recipe)
                                                                .ThenInclude(r => r.Ingredients)
                                                                .ThenInclude(i => i.Unit)
                                                            .Include(gr => gr.Recipe)
                                                                .ThenInclude(r => r.Images)
                                                            .Include(gr => gr.Recipe)
                                                                .ThenInclude(r => r.Steps)
                                                            .Include(gr => gr.SharedByUser)
                                                            .Where(gr => gr.GroupId == id)
                                                            .ToListAsync();
            if (groupRecipes.Count < 1)
                return NotFound();

            List<GroupRecipeDto> groupRecipeDtos = RecipeMapper.ToGroupRecipeDtos(groupRecipes);

            if (groupRecipeDtos.Count > 0)
                return Ok(groupRecipeDtos);

            return NotFound();
        }

        [HttpGet]
        [Route("get-recent")]
        public async Task<IActionResult> GetRecentGroupRecipes(int groupId, int numberOfRecipes)
        {
            List<GroupRecipeEntity> filterdGorupRecipes = await _dataContext.GroupRecipes
                                                                                     .OrderByDescending(gr => gr.CreatedAt)
                                                                                     .Take(numberOfRecipes)
                                                                                     .Where(gr => gr.GroupId == groupId)
                                                                                     .ToListAsync();

            if (filterdGorupRecipes.Count < 1)
                return NotFound();

            List<int> recipeIds = filterdGorupRecipes.Select(r => r.RecipeId).Distinct().ToList();

            List<RecipeEntity> recipeEntities = await _dataContext.Recipes
                                                                  .Include(r => r.Ingredients)
                                                                    .ThenInclude(i => i.Unit)
                                                                  .Include(r => r.Steps)
                                                                  .Include(r => r.Images)
                                                                  .Where(r => recipeIds.Contains(r.Id))
                                                                  .ToListAsync();

            if (recipeEntities.Count < 1)
                return NotFound();

            List<RecipeDtoGet> recentRecipesDtos = RecipeMapper.ToRecipeDtos(recipeEntities);

            return Ok(recentRecipesDtos);
        }
    }
}
