
using meal_menu_api.Helpers;
using meal_menu_api.Managers;

namespace meal_menu_api.Config
{
    public static class ServiceConfiguration
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<AuthManager>();
            services.AddScoped<RecipeManager>();
            services.AddScoped<ImageManager>();
            services.AddScoped<ToolBox>();
            services.AddScoped<UnitConversionManager>();
            services.AddScoped<ShoppingListManager>();
        }
    }
}
