using meal_menu_api.Dtos.Account;
using meal_menu_api.Entities.Account;

namespace meal_menu_api.Mappers
{
    public static class UserMapper
    {
        public static AppUser ToAppUser(RegisterUserDto registerDto)
        {
            var newUser = new AppUser
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                UserName = registerDto.Email,
                EmailConfirmed = true,
            };

            return newUser;
        }

        public static UserDto ToUserDto(AppUser existingUser)
        {
            var userDto = new UserDto
            {
                FirstName = existingUser.FirstName!,
                LastName = existingUser.LastName!,
                Email = existingUser.Email!,
                UserName = existingUser.UserName!,
                PhoneNumber = existingUser.PhoneNumber ?? null,
                EmailConfirmed = existingUser.EmailConfirmed,
                TowFactorEnabeld = existingUser.TwoFactorEnabled,
                LastLogin = DateTime.Now
            };

            return userDto;
        }
    }
}
