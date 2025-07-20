using meal_menu_api.Database.Context;
using meal_menu_api.Dtos.Groups;
using meal_menu_api.Entities;
using meal_menu_api.Filters;
using meal_menu_api.Models.Enums;
using meal_menu_api.Models.Forms;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace meal_menu_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [UseApiKey]
    [Authorize]
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

            var invitedUser = await _dataContext.Users.FirstOrDefaultAsync(u => u.Email == model.Email);

            if (invitedUser == null)
                return NotFound("user not found");

            var invitedByUser = _dataContext.Users
                .FirstOrDefault(u => u.Email == User.Identity!.Name);

            var group = await _dataContext.Groups.FirstOrDefaultAsync(g => g.Id == model.GroupId);

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
                Message = $"{invitedByUser.FirstName} has invited you to join {group.Name}",
            };

            _dataContext.GroupInvitations.Add(invitation);
            await _dataContext.SaveChangesAsync();

            var invitationDto = new GroupInvitationDto
            {
                InvitationId = invitation.Id.ToString(),

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

        [HttpPost]
        [Route("{invitationId}/accept")]
        public async Task<IActionResult> AcceptInvitation(string invitationId)
        {
            if (string.IsNullOrEmpty(invitationId))
                return BadRequest();

            var invitation = await _dataContext.GroupInvitations
                .Include(i => i.Group)
                .Include(i => i.InvitedUser)
                .FirstOrDefaultAsync(g => g.Id.ToString() == invitationId);

            if (invitation == null)
                return NotFound();

            invitation.Status = InvitationStatus.Accepted;
            invitation.RespondedAt = DateTime.Now;

            var newGroupMember = new GroupMemberEntity
            {
                GroupId = invitation.GroupId,
                Group = invitation.Group,
                UserId = invitation.InvitedUserId,
                User = invitation.InvitedUser,
                Role = GroupRole.Member,
            };

            _dataContext.GroupMembers.Add(newGroupMember);
            await _dataContext.SaveChangesAsync();

            return Ok();
        }

        [HttpPatch]
        [Route("{invitationId}/decline")]
        public async Task<IActionResult> DeclineInvitation(string invitationId)
        {
            if (string.IsNullOrEmpty(invitationId))
                return BadRequest();

            var invitation = await _dataContext.GroupInvitations
                .FirstOrDefaultAsync(i => i.Id.ToString() == invitationId);

            if (invitation == null)
                return NotFound();

            invitation.Status = InvitationStatus.Declined;
            invitation.RespondedAt = DateTime.Now;

            await _dataContext.SaveChangesAsync();

            return Ok();
        }

        [HttpGet]
        [Route("pending")]
        public async Task<IActionResult> GetInvitations()
        {
            var email = User.Identity?.Name;
            if (string.IsNullOrEmpty(email))
                return Unauthorized();

            var invites = await _dataContext.GroupInvitations
                .Include(i => i.InvitedByUser)
                .Where(i => i.InvitedUser.Email == email && i.Status == InvitationStatus.Pending)
                .ToListAsync();

            if (!invites.Any())
                return NoContent();

            var invitesDto = invites.Select(invite => new GroupInvitationDto
            {
                InvitationId = invite.Id.ToString(),

                InvitedByUser = new InvitedByUserDto
                {
                    FirstName = invite.InvitedByUser.FirstName,
                    LastName = invite.InvitedByUser.LastName,
                    Email = invite.InvitedByUser.Email ?? "",
                    UserName = invite.InvitedByUser.UserName ?? ""
                },
                Message = invite.Message,
                SentAt = invite.SentAt,
                Status = invite.Status,
                CreatedAt = invite.CreatedAt
            }).ToList();

            return Ok(invitesDto);
        }
    }
}
