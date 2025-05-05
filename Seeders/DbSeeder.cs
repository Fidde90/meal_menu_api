using meal_menu_api.Context;
using meal_menu_api.Entities;

namespace meal_menu_api.Seeders
{
    public class DbSeeder
    {
        public static void SeedUnits(DataContext context)
        {
            var units = new List<string> { "ml", "cl", "dl", "l", "mg", "g", "hg", "kg" };

            foreach (var unitName in units)
            {
                if (!context.Units.Any(u => u.Name == unitName))
                {
                    context.Units.Add(new UnitEntity { Name = unitName, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
                }
            }

            context.SaveChanges();
        }
    }
}
