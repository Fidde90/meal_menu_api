using meal_menu_api.Database.Context;
using meal_menu_api.Dtos.Groups;
using meal_menu_api.Entities;
using meal_menu_api.Filters;
using meal_menu_api.Models.Enums;
using meal_menu_api.Models.Forms;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace meal_menu_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [UseApiKey]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class InvitationController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public InvitationController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateInvitation(CreateInvitationModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var invitedUser = _dataContext.Users.FirstOrDefault(u => u.Email == model.Email);

            if (invitedUser == null)
                return NotFound("user not found");

            var invitedByUser = _dataContext.Users
                .FirstOrDefault(u => u.Email == User.Identity!.Name);

            var group = _dataContext.Groups.FirstOrDefault(g => g.Id == model.GroupId);

            if (invitedByUser == null || group == null)
                return BadRequest("Inviting user or group not found.");

            var existing = _dataContext.GroupInvitations
                    .Any(i => i.GroupId == group.Id && i.InvitedUserId == invitedUser.Id && 
                    i.Status == InvitationStatus.Pending);

            if (existing)
                return Conflict("User is already invited.");

            var invitation = new GroupInvitationEntity
            {
                InvitedByUserId = invitedByUser.Id,
                InvitedByUser = invitedByUser,
                InvitedUserId = invitedUser.Id,
                InvitedUser = invitedUser,
                GroupId = group.Id,
                Group = group,
                Message = $"{invitedByUser.FirstName} has invited you to {group.Name}",
            };

            _dataContext.GroupInvitations.Add(invitation);
            await _dataContext.SaveChangesAsync();

            var invitationDto = new GroupInvitationDto
            {
                InvitedByUser = new InvitedByUserDto
                {
                    FirstName = invitation.InvitedByUser.FirstName,
                    LastName = invitation.InvitedByUser.LastName,
                    Email = invitation.InvitedByUser.Email ?? "",
                    UserName = invitation.InvitedByUser.UserName ?? ""
                },

                InvitedUser = new InvitedUserDto
                {
                    FirstName = invitation.InvitedUser.FirstName,
                    LastName = invitation.InvitedUser.LastName,
                    Email = invitation.InvitedUser.Email ?? "",
                    UserName = invitation.InvitedUser.UserName ?? "",
                    status = invitation.Status.ToString().ToLower()
                },

                Message = invitation.Message,
                SentAt = invitation.SentAt,
                Status = invitation.Status,
                CreatedAt = invitation.CreatedAt
            };

            return Ok(invitationDto);
        }
    }
}
