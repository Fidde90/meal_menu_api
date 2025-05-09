using meal_menu_api.Database.Context;
using meal_menu_api.Dtos;
using meal_menu_api.Entities;
using meal_menu_api.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
                var user = _dataContext.Users.FirstOrDefault(u => u.Email == registerDto.Email);
                if (user != null)
                    return Conflict("A user with this email already exists");

                var existingUser = await _dataContext.Set<AppUser>().FindAsync(registerDto.Email);
                if (existingUser != null)
                    return Conflict();

                var newUser = new AppUser
                {
                    FirstName = registerDto.FirstName,
                    LastName = registerDto.LastName,
                    Email = registerDto.Email,
                    UserName = registerDto.Email,
                    EmailConfirmed = true,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
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
                        FirstName        = existingUser.FirstName!,
                        LastName         = existingUser.LastName!,
                        Email            = existingUser.Email!,
                        UserName         = existingUser.UserName!,
                        PhoneNumber      = existingUser.PhoneNumber ?? null,
                        EmailConfirmed   = existingUser.EmailConfirmed,
                        TowFactorEnabeld = existingUser.TwoFactorEnabled,
                        LastLogin        = DateTime.Now
                    };

                    existingUser.LastLogin = DateTime.UtcNow;
                    await _userManager.UpdateAsync(existingUser);
                    return Ok(new { User = userDto, Token = jwtToken });
                }
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("change-password")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { user = changePasswordDto, message = "invalid input!" });

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return NotFound(new { message = "user not found" });

            if (!await _userManager.CheckPasswordAsync(user, changePasswordDto.Password))
                return Unauthorized();

            user.UpdatedAt = DateTime.Now;
            var passwordChanged = await _userManager.ChangePasswordAsync(user, changePasswordDto.Password, changePasswordDto.NewPassword);

            if (passwordChanged.Succeeded)
                return Ok();

            return BadRequest(new { message = "Failed to delete user" });
        }

        [HttpPost("delete")]
        [Route("delete")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> DeleteAccount(DeleteAccountDto deleteAccountdto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { user = deleteAccountdto, message = "invalid input!" });

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return NotFound(new { message = "user not found" });

            if (!await _userManager.CheckPasswordAsync(user, deleteAccountdto.Password))
                return Unauthorized();

            var deleted = await _userManager.DeleteAsync(user);

            if (deleted.Succeeded)
                return NoContent();

            return BadRequest(new { message = "Failed to delete user" });

        }
    }
}
