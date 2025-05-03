using meal_menu_api.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace meal_menu_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class AccountController : ControllerBase
    {
        [HttpPost]
        [Route("register")]
        public IActionResult RegisterUser([FromBody]RegisterUserDto registerModel)
        {
            if(!ModelState.IsValid)
                return BadRequest("modell suger");
                



            return Ok("modell är ok");
        } 
    }
}
