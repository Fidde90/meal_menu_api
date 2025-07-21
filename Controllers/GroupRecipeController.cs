using meal_menu_api.Database.Context;
using meal_menu_api.Dtos;
using meal_menu_api.Dtos.Groups;
using meal_menu_api.Entities;
using meal_menu_api.Entities.Account;
using meal_menu_api.Entities.Groups;
using meal_menu_api.Entities.Recipes;
using meal_menu_api.Filters;
using meal_menu_api.Managers;
using meal_menu_api.Mappers;
using meal_menu_api.Models.Forms;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;

namespace meal_menu_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [UseApiKey]
    [Authorize]
    public class GroupRecipeController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly RecipeManager _recipeManager;

        public GroupRecipeController(DataContext dataContext, UserManager<AppUser> userManager, RecipeManager recipeManager)
        {
            _dataContext = dataContext;
            _userManager = userManager;
            _recipeManager = recipeManager;
        }

        [HttpPost]
        public async Task<IActionResult> CreateGroupRecipe(CreateGroupRecipeModel requestModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            AppUser? user = await _userManager.GetUserAsync(User);

            if (user == null)
                return Unauthorized();

            RecipeEntity? recipe = await _dataContext.Recipes
                                                      .Include(r => r.Ingredients)
                                                      .Include(r => r.Images)
                                                      .Include(r => r.Steps)
                                                      .FirstOrDefaultAsync(r => r.Id == requestModel.RecipeId);

            if (recipe == null)
                return NotFound("recipe not found");

            //alla användarens grupper
            List<GroupEntity> allUserGroups = await _dataContext.Groups
                .Include(g => g.Members.Where(m => m.UserId == user.Id))
                .Include(g => g.GroupRecipes)
                .ToListAsync();

            if (!allUserGroups.Any())
                return NotFound("no user groups found");

            // 2. Filtrera ut grupper där receptet ska tas bort
            var groupRecipesToRemove = allUserGroups
                .Where(group => !requestModel.GroupIds.Contains(group.Id)) // inte med i listan, alla grupper som inte var med i dto:n
                .SelectMany(group => group.GroupRecipes
                    .Where(gr => gr.RecipeId == requestModel.RecipeId))    // men bara som har receptet
                .ToList();

            // 3. Ta bort recepten från dessa grupper         
            if (groupRecipesToRemove.Count > 0)
            {
                _dataContext.GroupRecipes.RemoveRange(groupRecipesToRemove);
                await _dataContext.SaveChangesAsync();
            }

            var groupRecipesToAdd = allUserGroups.Where(group => requestModel.GroupIds.Contains(group.Id));

            foreach (GroupEntity group in groupRecipesToAdd)
            {
                if (group.GroupRecipes.Any(x => x.RecipeId == recipe.Id))
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

            var user = await _userManager.GetUserAsync(User);
            string userId = user != null ? user.Id : "";
            List<GroupRecipeDto> groupRecipeDtos = RecipeMapper.ToGroupRecipeDtos(groupRecipes, userId);

            if (groupRecipeDtos.Count > 0)
                return Ok(groupRecipeDtos);

            return NotFound();
        }

        [HttpGet]
        [Route("get-recent")]
        public async Task<IActionResult> GetRecentGroupRecipes(int groupId, int n)
        {
            List<GroupRecipeEntity> filterdGroupRecipes = await _dataContext.GroupRecipes
                                                                                    .Include(gr => gr.Recipe)
                                                                                        .ThenInclude(r => r.Ingredients)
                                                                                        .ThenInclude(i => i.Unit)
                                                                                    .Include(gr => gr.Recipe)
                                                                                        .ThenInclude(r => r.Images)
                                                                                    .Include(gr => gr.Recipe)
                                                                                        .ThenInclude(r => r.Steps)
                                                                                    .Include(gr => gr.SharedByUser)
                                                                                    .Where(gr => gr.GroupId == groupId)
                                                                                    .OrderByDescending(gr => gr.CreatedAt)
                                                                                    .Take(n)                                                                          
                                                                                    .ToListAsync();

            if (filterdGroupRecipes.Count < 1)
                return NotFound();

            var user = await _userManager.GetUserAsync(User);
            string userId = user != null ? user.Id : "";
            List<GroupRecipeDto> recentRecipesDtos = RecipeMapper.ToGroupRecipeDtos(filterdGroupRecipes, userId);

            return Ok(recentRecipesDtos);
        }

        [HttpGet]
        [Route("share-with-group/{recipeId}")]
        public async Task<IActionResult> GetGroupsWithRecipeSharedStatus(string recipeId)
        {
            if (string.IsNullOrEmpty(recipeId))
                return BadRequest();

            AppUser? user = await _userManager.GetUserAsync(User!);

            if (user == null)
                return Unauthorized("user not found");

            var result = await _dataContext.GroupMembers
                                                    .Where(gm => gm.UserId == user.Id)
                                                    .Select(gm => new RecipeInGroupDto
                                                    {
                                                        GroupId = gm.Group.Id,
                                                        GroupName = gm.Group.Name,
                                                        RecipeInGroup = gm.Group.GroupRecipes.Any(gr => gr.RecipeId.ToString() == recipeId)
                                                    })
                                                    .ToListAsync();

            return Ok(result);
        }

        [HttpPost]
        [Route("{recipeId}/clone")]
        public async Task<IActionResult> CloneGroupRecipeToUserRecipe(int recipeId)
        {
            var user = await _userManager.GetUserAsync(User);

            var originalRecipe = await _dataContext.Recipes
                                                .Include(r => r.Ingredients)
                                                    .ThenInclude(r => r.Unit)
                                                .Include(r => r.Images)
                                                .Include(r => r.Steps)
                                                .FirstOrDefaultAsync(r => r.Id == recipeId);
            if (originalRecipe == null)
                return NotFound("recipe not found");

            var clonedRecipe = await _recipeManager.CloneRecipe(originalRecipe, user!);
            var recipeDto = RecipeMapper.ToRecipeDtoGet(clonedRecipe);

            if(clonedRecipe != null)
                return Ok(recipeDto);

            return StatusCode(500);
        }
    }
}
