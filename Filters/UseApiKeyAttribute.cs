using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace meal_menu_api.Filters
{
    [AttributeUsage(validOn:AttributeTargets.Class | AttributeTargets.Method)]
    public class UseApiKeyAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //Gets the appsettings.json file
            var config = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            var apiKey = config.GetValue<string>("ApiKey:Secret"); // hämtar upp nyckeln

            if(!context.HttpContext.Request.Headers.TryGetValue("key", out var providedKey))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            if (!apiKey!.Equals(providedKey))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            await next();
        }
    }
}
