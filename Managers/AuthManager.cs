using meal_menu_api.Entities;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace meal_menu_api.Managers
{
    public class AuthManager
    {
        private readonly IConfiguration _configuration;

        public AuthManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetToken(AppUser user)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]!);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Email, user.Email!),
                    new Claim(ClaimTypes.Name, user.UserName!)
                };

                var tokenDescripter = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddDays(30),
                    Issuer = _configuration["Jwt:Issuer"],
                    Audience = _configuration["Jwt:Audience"],
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescripter);
                var tokenString = tokenHandler.WriteToken(token);

                return tokenString;
            }
            catch (Exception e) { Debug.WriteLine("Error in GetToken(), AuthController: " + e.Message); }
            return null!;
        }
    }
}
