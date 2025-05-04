
using meal_menu_api.Managers;

namespace meal_menu_api.Config
{
    public static class ServiceConfiguration
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<AuthManager>();
        }
    }
}
