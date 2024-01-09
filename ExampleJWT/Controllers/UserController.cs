using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ExampleJWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

  
    
     
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public UserController (IConfiguration configuration)
        {
            _configuration = configuration;
        }

        List<User> users = new List<User>
        {
            new User { Id = 1, UserName = "arun" , Password = "12345656", Role = "user"},
            new User { Id = 2, UserName = "thameem", Password = "1234567890", Role = "admin"}
        };


        [HttpPost("login")]
        public IActionResult Login([FromBody] User credentials)
        {
            try
            {
                if (credentials == null )
                {
                    return BadRequest("Invalid login credentials.");
                }

                var userCheeck = users.FirstOrDefault(s => s.Role == "admin");

               
                if (userCheeck == null)
                {
                    return Unauthorized("Invalid username or password.");
                }

               
                string token = GenerateJwtToken(userCheeck);

                
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }



        private string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Role, user.Role)
        };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

       


    }
}
