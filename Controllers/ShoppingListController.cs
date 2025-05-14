using meal_menu_api.Database.Context;
using meal_menu_api.Entities;
using meal_menu_api.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace meal_menu_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [UseApiKey]

    public class ShoppingListController : ControllerBase
    {

        private readonly DataContext _dataContext;

        public ShoppingListController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet]
        [Route("get")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetShoppingList()
        {
            var user = await _dataContext.Users.FirstOrDefaultAsync();

            var dinnerSchedule = await _dataContext.DinnerSchedules
                .Include(ds => ds.Dinners)
                .FirstOrDefaultAsync(ds => ds.UserId == user!.Id);

            // Plocka ut alla recipeId från dinners
            var recipeIds = dinnerSchedule!.Dinners
                .Select(d => d.RecipeId)
                .Distinct()
                .ToList();

            // Hämta alla ingredienser som hör till dessa recept
            var ingredients = await _dataContext.Ingredients
                .Where(i => recipeIds.Contains(i.RecipeId))
                .Include(i => i.Unit) // Om du även vill ha med enheten
                .ToListAsync();

            var json = JsonConvert.SerializeObject(ingredients, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = Formatting.Indented
            });

            Console.WriteLine($"\n\n {json}");




            return Ok();
        }
    }
}
