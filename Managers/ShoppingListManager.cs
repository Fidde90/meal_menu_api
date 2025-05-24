using meal_menu_api.Database.Context;
using meal_menu_api.Entities;
using meal_menu_api.Models.Enums;
using meal_menu_api.Models;
using Microsoft.EntityFrameworkCore;
using meal_menu_api.Helpers;

namespace meal_menu_api.Managers
{
    public class ShoppingListManager
    {
        private readonly DataContext _dataContext;
        private readonly ToolBox _toolbox;
        private readonly UnitConversionManager _unitCM;
        public ShoppingListManager(DataContext dataContext, ToolBox toolbox, UnitConversionManager unitCM)
        {
            _dataContext = dataContext;
            _toolbox = toolbox;
            _unitCM = unitCM;
        }


        public async Task<ShoppingListEntity> CreateShoppingList(string email)
        {
            if (string.IsNullOrEmpty(email))
                return null!;

            var user = await _dataContext.Users
                        .Include(u => u.DinnerSchedules)
                            .ThenInclude(ds => ds.Dinners)
                        .Include(u => u.DinnerSchedules)
                            .ThenInclude(ds => ds.ShoppingList)
                                .ThenInclude(sl => sl!.Ingredients)
                        .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
                return null!;

            DateTime now = DateTime.Now;
            var dinnerSchedule = user.DinnerSchedules.FirstOrDefault(ds => (ds.StartsAtDate <= now) &&
                                                                                (ds.EndsAtDate > now));
            if (dinnerSchedule == null)
                return null!;

            if (dinnerSchedule.ShoppingList == null)
            {
                var shoppingList = new ShoppingListEntity
                {
                    User = user!,
                    UserId = user!.Id,
                    DinnerScheduleId = dinnerSchedule.Id,
                    DinnerSchedule = dinnerSchedule,
                    Status = ShoppingListStatus.Active,
                    Name = "",
                    Notes = "",
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
                    var convertedVolume = _unitCM.ConvertVolume(new ConvertModel(ingredient.Unit.Name, ingredient.Amount));

                    var ingredientInList = shoppingListIngredients
                                        .FirstOrDefault(i => string.Equals(i.Name, ingredient.Name, StringComparison.OrdinalIgnoreCase) &&
                                                             string.Equals(i.Description, ingredient.Description, StringComparison.OrdinalIgnoreCase) &&
                                                             string.Equals(i.Unit, convertedVolume.Unit, StringComparison.OrdinalIgnoreCase));

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

                List<ShoppingListIngredientEntity> convertedEntities = _unitCM.ConvertIngredientVolumes(shoppingListIngredients);

                shoppingList.Ingredients = convertedEntities;

                _dataContext.ShoppingLists.Add(shoppingList);
                await _dataContext.SaveChangesAsync();

                return shoppingList;
            }

            return dinnerSchedule.ShoppingList;
        }
    }
}
