using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace meal_menu_api.Controllers
{
    public class AuthController(IConfiguration configuration) : ControllerBase
    {
        private readonly IConfiguration _configuration = configuration;

        [HttpPost]
        [Route("token")]
        public IActionResult GetToken()
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]!);
                var tokenDescripter = new SecurityTokenDescriptor
                {
                    Expires = DateTime.UtcNow.AddMinutes(15),
                    Issuer = _configuration["Jwt:Issuer"],
                    Audience = _configuration["Jwt:Audience"],
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescripter);
                var tokenString = tokenHandler.WriteToken(token);

                return Ok(tokenString);
            }
            catch (Exception e) { Debug.WriteLine("Error: " + e.Message); }
            return Unauthorized();
        }
    }
}
