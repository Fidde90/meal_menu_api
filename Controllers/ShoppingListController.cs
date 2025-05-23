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

            var recipeIds = dinnerSchedule!.Dinners
                .Select(d => d.RecipeId)
                .Distinct()
                .ToList();

            var ingredients = await _dataContext.Ingredients
                .Where(i => recipeIds.Contains(i.RecipeId))
                .Include(i => i.Unit)
                .ToListAsync();


            //_dataContext.ShoppingLists.Add(shoppingList);
            _dataContext.ShoppingLists.Add(shoppingList);

            await _dataContext.SaveChangesAsync();
            //await _dataContext.SaveChangesAsync();


            var shoppinglistEntity = _dataContext.ShoppingLists.FirstOrDefault();

            var shoppingListDto = new ShoppingListDto

            {
                Id = shoppinglistEntity.Id,
                Name = shoppinglistEntity.Name,
                Notes = shoppinglistEntity.Notes,
                Status = shoppinglistEntity.Status,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            var list = shoppingListDto!.Ingredients;

            foreach (var item in ingredients)
            {
                //UnitEntity unit = _dataContext.Units.First(u => u.Id == item.UnitId);

                var convertetVolume = ConvertVolume(new ConvertModel(item.Unit.Name, item.Amount));

                if (list.Any(s => AreEqual(s.Name, item.Name) && AreEqual(s.Description, item.Description) && AreEqual(s.Unit, convertetVolume.Unit)))
                {
                    ShoppingListIngredientDto exist = 
                        list.First(l => 
                             AreEqual(l.Name, item.Name) && 
                             AreEqual(l.Description, item.Description) && 
                             AreEqual(l.Unit, convertetVolume.Unit)
                        );

                    exist.Amount = exist.Amount + convertetVolume.Volume;
                }
                else
                {
                    var newItem = new ShoppingListIngredientDto
                    {
                        Id = item.Id,
                        ShoppingListId = shoppingListDto.Id,
                        Description = item.Description,
                        Name = item.Name,
                        Amount = convertetVolume.Volume,
                        Unit = convertetVolume.Unit,
                        IsChecked = false,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    };

                    list.Add(newItem);
                }
            }

            List<ShoppingListIngredientDto> returnList = ConvertIngredientVolumes(list);

            return Ok(returnList);
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
                    case "g": return new ConvertModel("g", volume);
                }
            }

            return null!;
        }

        public List<ShoppingListIngredientDto> ConvertIngredientVolumes(List<ShoppingListIngredientDto> ingredients)
        {
            List<ShoppingListIngredientDto> convertedIngredients = [];

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

