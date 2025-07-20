using meal_menu_api.Database.Context;
using meal_menu_api.Dtos;
using meal_menu_api.Dtos.Groups;
using meal_menu_api.Entities;
using meal_menu_api.Filters;
using meal_menu_api.Managers;
using meal_menu_api.Models.Enums;
using meal_menu_api.Models.Forms;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace meal_menu_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [UseApiKey]
    [Authorize]
    public class GroupController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly ImageManager _imageManager;

        public GroupController(DataContext dataContext, ImageManager imageManager)
        {
            _dataContext = dataContext;
            _imageManager = imageManager;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateGroup(CreateGroupFormModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = _dataContext.Users.FirstOrDefault(u => u.Email == User.Identity!.Name);

            if (user == null)
                return Unauthorized("user not found");

            GroupEntity newGroup = new GroupEntity
            {
                OwnerId = user.Id,
                Name = model.Name,
                Description = model.Description,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _dataContext.Groups.Add(newGroup);
            await _dataContext.SaveChangesAsync();

            if (model.Icon != null)
            {
                string imageUrl = await _imageManager.SaveImage(model.Icon, newGroup.Id);
                newGroup.IconUrl = imageUrl;
            }

            GroupMemberEntity newMember = new GroupMemberEntity
            {
                GroupId = newGroup.Id,
                Group = newGroup,
                UserId = user.Id,
                User = user,
                Role = GroupRole.GroupOwner,
                JoinedAt = DateTime.Now,
            };

            newGroup.Members.Add(newMember);
            await _dataContext.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetGroup(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var group = await _dataContext.Groups
                                .Include(g => g.Invitations)
                                    .ThenInclude(i => i.InvitedUser)
                                .Include(g => g.Members)
                                    .ThenInclude(m => m.User)
                                .FirstOrDefaultAsync(g => g.Id.ToString() == id);

                if (group == null)
                    return NotFound();

          

                var groupDto = new GroupDto
                {
                    Id = group.Id,
                    Name = group.Name,
                    Description = group.Description,
                    IconUrl = group.IconUrl?.Replace("\\", "/")!,
                    CreatedAt = group.CreatedAt,
                    UpdatedAt = group.UpdatedAt,
                };

                List<GroupMemberDto> menbers = [];
                foreach (var member in group.Members)
                {
                    var memberDto = new GroupMemberDto
                    {
                        FirstName = member.User.FirstName,
                        LastName = member.User.LastName,
                        Email = member.User.Email ?? "",
                        UserName = member.User.UserName ?? "",
                        Role = member.Role.ToString(),
                        JoinedAt = member.JoinedAt,
                        LastLogin = member.User.LastLogin
                    };

                    menbers.Add(memberDto);
                }

                groupDto.Members = menbers;

                List<GroupInvitationDto> invites = [];
                foreach (var invitation in group.Invitations.Where(i => i.Status == InvitationStatus.Pending))
                {
                    var invitationDto = new GroupInvitationDto
                    {
                        InvitedByUser = new InvitedByUserDto 
                        { 
                            FirstName = invitation.InvitedByUser.FirstName,
                            LastName= invitation.InvitedByUser.LastName,
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

                    invites.Add(invitationDto);
                }

                groupDto.Invites = invites;


                return Ok(groupDto);
            }

            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllGroups()
        {
            var user = _dataContext.Users.FirstOrDefault(u => u.Email == User.Identity!.Name);

            if (user == null)
                return Unauthorized("user not found");

            List<GroupMemberEntity> groupsMember = await _dataContext.GroupMembers.Where(m => m.UserId == user.Id)
                                                                                            .Include(m => m.Group)
                                                                                             
                                                                                            .ToListAsync();
            UserGroupsDto groups = new();

            foreach (var group in groupsMember)
            {          
                List<GroupMemberDto> members = [];

                foreach (var member in group.Group.Members)
                {
                    members.Add(new GroupMemberDto
                    {
                        FirstName = member.User.FirstName,
                        LastName = member.User.LastName,
                        Email = member.User.Email ?? "",
                        UserName = member.User.UserName ?? "",
                        Role = member.Role.ToString(),
                        JoinedAt = member.JoinedAt,
                        LastLogin = member.User.LastLogin
                    });
                }

                groups.Groups.Add(
                    new GroupDto
                    {
                        Id = group.Group.Id,
                        Name = group.Group.Name,
                        Description = group.Group.Description,
                        IconUrl = group.Group.IconUrl?.Replace("\\", "/")!,
                        CreatedAt = group.Group.CreatedAt,
                        UpdatedAt = group.Group.UpdatedAt,
                        Members = members
                    }
                );
            }

            return Ok(groups);
        }

        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> UpdateGroup(UpdateGroupFormModel model)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingGroup = await _dataContext.Groups.FirstOrDefaultAsync(g => g.Id.ToString() == model.GroupId);

            if (existingGroup == null)
                return NotFound();

            existingGroup.Name = model.Name;
            existingGroup.Description = model.Description;
            existingGroup.UpdatedAt = DateTime.Now;
   

            if (model.DeleteIcon && model.Icon == null)
            {
                var deletedImage = await _imageManager.DeleteGroupImage(existingGroup.Id);
                var newImageUrl = await _imageManager.SaveImage(model.Icon!, Convert.ToInt32(model.GroupId));
                existingGroup.IconUrl = newImageUrl.Replace("\\", "/");
            }

            if(!model.DeleteIcon && model.Icon != null)
            {
                var deletedImage = await _imageManager.DeleteGroupImage(existingGroup.Id);
                var newImageUrl = await _imageManager.SaveImage(model.Icon!, Convert.ToInt32(model.GroupId));
                existingGroup.IconUrl = newImageUrl.Replace("\\", "/");
            }
          
            await _dataContext.SaveChangesAsync();

            return Ok(existingGroup);
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> DeleteGroup(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var group = _dataContext.Groups.FirstOrDefault(g => g.Id.ToString() == id);

                if (group == null)
                    return NotFound("group not found");

                _dataContext.Groups.Remove(group);
                await _dataContext.SaveChangesAsync();

                return NoContent();
            }

            return BadRequest();
        } 
    }
}
