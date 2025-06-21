using meal_menu_api.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace meal_menu_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [UseApiKey] 
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class GroupRecipeController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateGroupRecipe()
        {




            return Ok();
        }
    }
}
