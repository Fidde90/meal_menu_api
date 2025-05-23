using meal_menu_api.Database.Context;
using meal_menu_api.Dtos;
using meal_menu_api.Entities;
using meal_menu_api.Filters;
using meal_menu_api.Models;
using meal_menu_api.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace meal_menu_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [UseApiKey]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class ShoppingListController : ControllerBase
    {

        private readonly DataContext _dataContext;

        public ShoppingListController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet]
        [Route("get")]
        public async Task<IActionResult> GetShoppingList()
        {
            var user = await _dataContext.Users
                .Include(u => u.DinnerSchedules)
                    .ThenInclude(ds => ds.Dinners)
                .Include(u => u.DinnerSchedules)
                    .ThenInclude(ds => ds.ShoppingList)
                        .ThenInclude(sl => sl!.Ingredients)
                .FirstOrDefaultAsync(u => u.Email == User.Identity!.Name);

            if (user == null)
                return NotFound("user not found");

            DateTime now = DateTime.Now;
            var dinnerSchedule = user.DinnerSchedules.FirstOrDefault(ds => (ds.StartsAtDate <= now) &&
                                                                                (ds.EndsAtDate > now));
            if (dinnerSchedule == null)
                return NotFound("no active dinner schedule");

            if (dinnerSchedule.ShoppingList == null)
            {
                var shoppingList = new ShoppingListEntity
                {
                    User = user!,
                    UserId = user!.Id,
                    DinnerScheduleId = dinnerSchedule.Id,
                    DinnerSchedule = dinnerSchedule,
                    Status = ShoppingListStatus.Active,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                var includedRecipeIds = dinnerSchedule!.Dinners
                    .Select(d => d.RecipeId)
                    .Distinct()
                    .ToList();

                var ingredients = await _dataContext.Ingredients
                    .Where(i => includedRecipeIds.Contains(i.RecipeId))
                    .Include(i => i.Unit)
                    .ToListAsync();

                var shoppingListIngredients = new List<ShoppingListIngredientEntity>();

                foreach (var ingredient in ingredients)
                {
                    var convertedVolume = ConvertVolume(new ConvertModel(ingredient.Unit.Name, ingredient.Amount));

                    var ingredientInList = shoppingListIngredients
                                        .FirstOrDefault(i => AreEqual(i.Name, ingredient.Name) &&
                                                             AreEqual(i.Description, ingredient.Description) &&
                                                             AreEqual(i.Unit, convertedVolume.Unit));

                    if (ingredientInList != null)
                    {
                        ingredientInList.Amount += convertedVolume.Volume;
                    }
                    else
                    {
                        var newItem = new ShoppingListIngredientEntity
                        {
                            ShoppingListId = shoppingList.Id,
                            ShoppingList = shoppingList,
                            Description = ingredient.Description,
                            Name = ingredient.Name,
                            Amount = convertedVolume.Volume,
                            Unit = convertedVolume.Unit,
                            IsChecked = false,
                        };

                        shoppingListIngredients.Add(newItem);
                    }
                }

                List<ShoppingListIngredientEntity> convertedEntities = ConvertIngredientVolumes(shoppingListIngredients);

                shoppingList.Ingredients = convertedEntities;

                _dataContext.ShoppingLists.Add(shoppingList);
                await _dataContext.SaveChangesAsync();

                return Ok();
            }

            return Ok();
        }

        public static bool AreEqual(string? a, string? b)
        {
            return (a?.Trim().ToLower() ?? "") == (b?.Trim().ToLower() ?? "");
        }

        public ConvertModel ConvertVolume(ConvertModel model)
        {
            string unit = model.Unit.ToLower();

            if (!string.IsNullOrEmpty(unit))
            {
                if (unit == "kg" || unit == "hg" || unit == "g")
                    return ConvertToGram(model);
                else
                    return ConvertToMilliliter(model);
            }

            return null!;
        }

        public ConvertModel ConvertToMilliliter(ConvertModel model)
        {
            string unit = model.Unit.ToLower();
            double volume = model.Volume;

            if (!string.IsNullOrEmpty(unit) && volume > 0)
            {
                switch (unit)
                {
                    case "l": return new ConvertModel("ml", volume * 1000);
                    case "dl": return new ConvertModel("ml", volume * 100);
                    case "cl": return new ConvertModel("ml", volume * 10);
                    case "msk": return new ConvertModel("ml", volume * 15);
                    case "tsk": return new ConvertModel("ml", volume * 5);
                    case "ml":
                    case "krm": return new ConvertModel("ml", volume);
                    default:
                        return new ConvertModel(unit, volume);
                }
            }

            return null!;
        }

        public ConvertModel ConvertToGram(ConvertModel model)
        {
            string unit = model.Unit.ToLower();
            double volume = model.Volume;

            if (!string.IsNullOrEmpty(unit) && volume > 0)
            {
                switch (unit)
                {
                    case "kg": return new ConvertModel("g", volume * 1000);
                    case "hg": return new ConvertModel("g", volume * 100);
                    case "g" : return new ConvertModel("g", volume);
                }
            }

            return null!;
        }

        public List<ShoppingListIngredientEntity> ConvertIngredientVolumes(List<ShoppingListIngredientEntity> ingredients)
        {
            List<ShoppingListIngredientEntity> convertedIngredients = [];

            foreach (var ingredient in ingredients)
            {
                double volume = ingredient.Amount;
                string unit = ingredient.Unit.ToLower();

                if (unit == "ml")
                {
                    var conversions = new List<(double v, string u)>
                    {
                        (1000, "l"),
                        (100, "dl"),
                        (15, "msk"),
                        (5, "tsk"),
                        (1, "krm")
                    };

                    foreach (var (v, u) in conversions)
                    {
                        if (volume >= v)
                        {
                            ingredient.Amount = volume / v;
                            ingredient.Unit = u;
                            break;
                        }
                    }
                }
                else if (unit == "g")
                {
                    if (volume >= 1000)
                    {
                        ingredient.Amount = volume / 1000;
                        ingredient.Unit = "kg";
                    }
                }

                convertedIngredients.Add(ingredient);
            }

            return convertedIngredients;
        }
    }
}

