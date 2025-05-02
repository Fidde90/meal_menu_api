using Microsoft.AspNetCore.Mvc;

namespace meal_menu_api.Controllers
{
    public class UserController : ControllerBase
    {
        [HttpGet]
        [Route("/contact")]
        public IActionResult Index()
        {
            return Ok("We got contact!");
        }
    }
}
