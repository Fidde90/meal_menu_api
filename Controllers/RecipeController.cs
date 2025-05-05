using meal_menu_api.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace meal_menu_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecipeController : ControllerBase
    {

        [HttpPost]
        [Route("create")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> CreateRecipe(RecipeDto recipeDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();







            return Ok();
        }
    }
}
