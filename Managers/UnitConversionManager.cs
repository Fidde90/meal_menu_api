using meal_menu_api.Database.Context;
using meal_menu_api.Entities;
using meal_menu_api.Entities.ShoppingList;
using meal_menu_api.Models;

namespace meal_menu_api.Managers
{
    public class UnitConversionManager
    {
        private readonly DataContext _dataContext;

        public UnitConversionManager(DataContext dataContext)
        {
            _dataContext = dataContext;
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
