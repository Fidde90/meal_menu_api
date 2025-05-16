using meal_menu_api.Database.Context;
using meal_menu_api.Dtos;
using meal_menu_api.Dtos.Groups;
using meal_menu_api.Entities;
using meal_menu_api.Filters;
using meal_menu_api.Models.Enums;
using meal_menu_api.Models.Forms;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel;

namespace meal_menu_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [UseApiKey]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class GroupController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public GroupController(DataContext dataContext)
        {
            _dataContext = dataContext;
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
                string imageUrl = await SaveImage(model.Icon, newGroup.Id);
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

        [HttpGet]
        [Route("get")]
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
                var ownerEntity = _dataContext.Users.FirstOrDefault(u => u.Id == group.Group.OwnerId);
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
                var enumConvert = new EnumConverter(typeof(GroupRole));
                var owner = new GroupOwnerDto
                {
                    FirstName = ownerEntity?.FirstName,
                    LastName = ownerEntity?.LastName,
                    Email = ownerEntity?.Email,
                    UserName = ownerEntity?.UserName,
                    Role = GroupRole.GroupOwner.ToString(),
                    LastLogin = ownerEntity?.LastLogin,
                };

                groups.Groups.Add(
                    new GroupDto
                    {
                        Id = group.Group.Id,
                        Owner = owner,
                        Name = group.Group.Name,
                        Description = group.Group.Description,
                        IconUrl = group.Group.IconUrl,
                        CreatedAt = group.Group.CreatedAt,
                        UpdatedAt = group.Group.UpdatedAt,
                        Members = members
                    }
                );
            }

            return Ok(groups);
        }

        public async Task<string> SaveImage(IFormFile Image, int id)
        {
            string filePath = Path.Combine("Images/groups", $"{id.ToString()}_" + Image.FileName);
            using var stream = new FileStream(filePath, FileMode.Create);
            await Image.CopyToAsync(stream);
            return filePath;
        }
    }
}
