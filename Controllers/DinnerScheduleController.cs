using meal_menu_api.Database.Context;
using meal_menu_api.Dtos;
using meal_menu_api.Entities;
using meal_menu_api.Filters;
using meal_menu_api.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace meal_menu_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [UseApiKey]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class DinnerScheduleController(DataContext dataContext, ToolBox toolBox) : ControllerBase
    {
        private readonly DataContext _dataContext = dataContext;
        private readonly ToolBox _toolBox = toolBox;

        [HttpGet]
        [Route("get")] 
        public async Task<IActionResult> GetDinnerSchedule()
        {
            AppUser? user = _dataContext.Users.FirstOrDefault(u => u.Email == User.Identity!.Name);

            if (user == null)
                return NotFound("User not found");

            List<RecipeEntity> recipes = _dataContext.Recipes.Where(u => u.UserId == user!.Id).Include(r => r.Images)
                .OrderBy(r => r.RotationPoints).ToList();

            if (recipes.Count < 7)
                return BadRequest("a minimum of 7 recipes required for a dinner schedule");

            List<RecipeEntity> recipesToShuffle= recipes.Take(10).ToList();
            List<RecipeEntity> selectedRecipes = _toolBox.ShuffleArray(recipesToShuffle).Take(7).ToList();

            for (int i = 0; i < selectedRecipes.Count; i++)
            {
                selectedRecipes[i].RotationPoints += 25;

                if (selectedRecipes[i].RotationPoints >= 100)
                    selectedRecipes[i].RotationPoints = 0;
            }

            foreach (var recipe in recipes.Except(selectedRecipes))
                recipe.RotationPoints = Math.Min(recipe.RotationPoints + 25, 100);

            await _dataContext.SaveChangesAsync();

            DateTime Starts = DateTime.Today.AddDays(((int)DayOfWeek.Monday - (int)DateTime.Today.DayOfWeek + 7) % 7);
            DateTime Ends = Starts.AddDays(6);
            DinnerScheduleEntity newSchedule = new()
            {
                UserId = user!.Id,
                User = user,
                StartsAtDate = Starts,
                EndsAtDate = Ends,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };

            _dataContext.DinnerSchedules.Add(newSchedule);

            var schedule = newSchedule;
            await _dataContext.SaveChangesAsync();

            var scheduleDto = new DinnerScheduleDto()
            {
                StartsAtDate = schedule.StartsAtDate,
                EndsAtDate = schedule.EndsAtDate,
                CreatedAt = schedule.CreatedAt,
                UpdatedAt = schedule.UpdatedAt
            };

            List<DinnerEntity> dinners = new();

            for (int i = 0; i < selectedRecipes.Count; i++)
            {
                var newDinner = new DinnerEntity
                {
                    DinnerScheduleId = schedule.Id,
                    RecipeId = selectedRecipes[i].Id,
                    EatAt = schedule.StartsAtDate.AddDays(i),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                };

                dinners.Add(newDinner);
                schedule.Dinners.Add(newDinner);
            }

            await _dataContext.SaveChangesAsync();

            foreach (var dinner in dinners)
            {
                var recipe = selectedRecipes.First(r => r.Id == dinner.RecipeId);

                DinnerDto newDinnerDto = new()
                {
                    Id = dinner.Id,
                    RecipeId = recipe.Id,
                    Name = recipe.Name,
                    Description = recipe.Description,
                    Ppl = recipe.Ppl,
                    ImageUrl = recipe.Images.FirstOrDefault()?.ImageUrl.Replace("\\", "/")! ?? "",
                    EatAt = dinner.EatAt,
                    CreatedAt = dinner.CreatedAt,
                    UpdatedAt = dinner.UpdatedAt,
                };

                scheduleDto.Dinners.Add(newDinnerDto);
            }

            return Ok(scheduleDto);
        }
    }
}

