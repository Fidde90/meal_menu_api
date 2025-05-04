using meal_menu_api.Context;
using meal_menu_api.Dtos;
using meal_menu_api.Entities;
using meal_menu_api.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace meal_menu_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly DataContext _dataContext;
        private readonly AuthManager _authManager;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, DataContext dataContext, AuthManager authManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _dataContext = dataContext;
            _authManager = authManager;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> RegisterUser(RegisterUserDto registerDto)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _dataContext.Set<AppUser>().FindAsync(registerDto.Email);
                if (existingUser != null)
                    return Conflict();

                var newUser = new AppUser
                {
                    FirstName = registerDto.FirstName,
                    LastName = registerDto.LastName,
                    Email = registerDto.Email,
                    UserName = registerDto.Email,
                    EmailConfirmed = true
                };

                var createUser = await _userManager.CreateAsync(newUser, registerDto.Password);
                if (createUser.Succeeded)
                {
                    return Created();
                }
                else
                {
                    var errors = string.Join("; ", createUser.Errors.Select(e => e.Description));
                    return BadRequest($"User creation failed: {errors}");
                }
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginUser(LoginDto loginDto)
        {
            if (ModelState.IsValid)
            {
                AppUser? existingUser = await _userManager.FindByEmailAsync(loginDto.Email);

                if (existingUser == null)
                    return Unauthorized();

                var signIn = await _signInManager.PasswordSignInAsync(loginDto.Email, loginDto.Password, false, false);
                if (!signIn.Succeeded)
                    return BadRequest();

                string jwtToken = _authManager.GetToken(existingUser);
                if (jwtToken != null)
                {
                    var userDto = new UserDto
                    {
                        FirstName = existingUser.FirstName ?? null,
                        LastName = existingUser.LastName ?? null,
                        Email = existingUser.Email ?? null,
                        UserName = existingUser.UserName ?? null,
                        PhoneNumber = existingUser.PhoneNumber ?? null,
                        EmailConfirmed = existingUser.EmailConfirmed,
                        TowFactorEnabeld = existingUser.TwoFactorEnabled,
                        LastLogin = DateTime.UtcNow
                    };

                    return Ok(new { User = userDto, Token = jwtToken });
                }

            }
            return BadRequest();
        }


        [HttpDelete]
        [Route("delete")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> DeleteUser()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                var deleted = await _userManager.DeleteAsync(user);
                if (deleted.Succeeded)
                    return NoContent();
            }
            return NotFound(new {message = "user not found" });
        }
    }
}
