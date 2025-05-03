using meal_menu_api.Context;
using meal_menu_api.Dtos;
using meal_menu_api.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace meal_menu_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly DataContext _dataContext;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, DataContext dataContext)
        {
            _userManager   = userManager;
            _signInManager = signInManager;
            _dataContext   = dataContext;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> RegisterUser(RegisterUserDto registerModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingUser = await _dataContext.Set<AppUser>().FindAsync(registerModel.Email);
            if (existingUser != null)
                return Conflict();

            var newUser = new AppUser
            {
                FirstName = registerModel.FirstName,
                LastName = registerModel.LastName,
                Email = registerModel.Email,
                UserName = registerModel.Email,
                EmailConfirmed = true
            };

            var createdUser = await _userManager.CreateAsync(newUser, registerModel.Password);
            if (createdUser.Succeeded)
            {
                return Created(); // eller return CreatedAtAction(...) med info
            }
            else
            {
                var errors = string.Join("; ", createdUser.Errors.Select(e => e.Description));
                return BadRequest($"User creation failed: {errors}");
            }

            return StatusCode(500);
        }
    }
}
