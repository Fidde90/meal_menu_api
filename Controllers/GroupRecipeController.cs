using meal_menu_api.Database.Context;
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

                var newGroupRecipe = new GroupRecipeEntity
                {
                    GroupId = group.Id,
                    Group = group,
                    SharedByUserId = user.Id,
                    SharedByUser = user,
                    RecipeId = recipe.Id,
                    Recipe = recipe
                };

                await _dataContext.GroupRecipes.AddAsync(newGroupRecipe);
            }

            await _dataContext.SaveChangesAsync();

            return Ok();
        }

        [HttpGet]
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

            List<GroupRecipeDto> groupRecipeDtos = RecipeMapper.MapFullGroupRecipeDtos(groupRecipes);

            if (groupRecipeDtos.Count > 0)
                return Ok(groupRecipeDtos);

            return NotFound();

        }
    }
}
