using Microsoft.AspNetCore.Mvc;

namespace meal_menu_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet]
        [Route("contact")]
        public IActionResult Index()
        {
            return Ok("We got contact!");
        }
    }
}
