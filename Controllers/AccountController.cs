using meal_menu_api.Database.Context;
using meal_menu_api.Dtos;
using meal_menu_api.Dtos.Account;
using meal_menu_api.Entities.Account;
using meal_menu_api.Entities.Recipes;
using meal_menu_api.Managers;
using meal_menu_api.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
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

                var createUser = await _userManager.CreateAsync(UserMapper.ToAppUser(registerDto), registerDto.Password);

                if (createUser.Succeeded)
                    return Created();
                else
                    return BadRequest($"User creation failed: { string.Join("; ", createUser.Errors.Select(e => e.Description))}");
            }

            return BadRequest();
        }

        [HttpGet]
        [Route("hej")]
        public async Task<IActionResult> Hej()
        {
            return Ok("yo mannen!");
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
                    var userDto = UserMapper.ToUserDto(existingUser);
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

            var invitations = await _dataContext.GroupInvitations
                                .Where(i => i.InvitedUserId == user.Id || i.InvitedByUserId == user.Id)
                                .ToListAsync();

            _dataContext.GroupInvitations.RemoveRange(invitations);
            await _dataContext.SaveChangesAsync();

            var deleted = await _userManager.DeleteAsync(user);

            if (deleted.Succeeded)
                return Ok();

            return BadRequest(new { message = "Failed to delete user" });
        }
    }
}
