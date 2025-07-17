using meal_menu_api.Dtos.Account;
using meal_menu_api.Entities.Account;
using meal_menu_api.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace meal_menu_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;

        public UserController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        [Route("me")]
        [UseApiKey]
        [Authorize]
        public async Task<IActionResult> GeCurrenttUser()
        {
            AppUser? user = await _userManager.GetUserAsync(User);

            if (user != null)
                return Ok(new { user });

            return NotFound();
        }

        [HttpPut]
        [UseApiKey]
        [Authorize]
        public async Task<IActionResult> UpdateUser(UpdateUserDto updateUserDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { user = updateUserDto, message = "invalid input!" });

            var existingUserInfo = await _userManager.GetUserAsync(User);
            if (existingUserInfo == null)
                return NotFound(new { user = updateUserDto, message = "user not found!" });

            existingUserInfo.FirstName = updateUserDto.FirstName!;
            existingUserInfo.LastName = updateUserDto.LastName!;
            existingUserInfo.Email = updateUserDto.Email!;
            existingUserInfo.UserName = updateUserDto.Email;
            existingUserInfo.PhoneNumber = updateUserDto.PhoneNumber!;
            existingUserInfo.EmailConfirmed = true;
            existingUserInfo.TwoFactorEnabled = updateUserDto.TowFactorEnabeld;
            existingUserInfo.UpdatedAt = DateTime.Now;

            var updated = await _userManager.UpdateAsync(existingUserInfo);
            if (updated.Succeeded)
                return Ok(new { user = existingUserInfo, message = "user updated successfully!" });

            return StatusCode(500, "something went wrong, try again later...");
        }
    }
}
