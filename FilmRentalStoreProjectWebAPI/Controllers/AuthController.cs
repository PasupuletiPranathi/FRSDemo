using FilmRentalStoreProjectREPO.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace FilmRentalStoreProjectWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUser _userRepository;
        private readonly IConfiguration _configuration;

        public AuthController(IUser userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            if (loginDto == null)
                return BadRequest(new { message = "Invalid request data!" });

            // Fetch user from database
            var user = await _userRepository.GetUserByUsernameAndPasswordAsync(loginDto.Username, loginDto.Password);
            if (user == null)
                return Unauthorized(new { message = "Invalid credentials" });

            // If the user is a Staff, validate the Email
            if (user.Role.RoleName.ToLower() == "staff")
            {
                var staff = await _userRepository.GetStaffByUsernameAsync(loginDto.Username);
                if (staff == null || string.IsNullOrEmpty(staff.Email) || staff.Email.ToLower() != loginDto.Email.ToLower())
                {
                    return Unauthorized(new { message = "Invalid email for staff user!" });
                }
            }

            // Create JWT token
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.Username),
        new Claim(ClaimTypes.Role, user.Role.RoleName)
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            // **Return Email for Staff, only Username & Password for Users**
            if (user.Role.RoleName.ToLower() == "staff")
            {
                return Ok(new
                {
                    message = "Login Successful",
                    token = tokenString,
                    username = user.Username,
                    password = user.Password,
                    email = loginDto.Email
                });
            }
            else
            {
                return Ok(new
                {
                    message = "Login Successful",
                    token = tokenString,
                    username = user.Username,
                    password = user.Password
                });
            }
        }


    }
}
