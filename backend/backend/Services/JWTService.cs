using backend.Models;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace backend.Services
{
    public class JWTService
    {
        private readonly IConfiguration _configuration;

        public JWTService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateAccessToken(string userId, string userName, string userEmail)
        {
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, userId),
                new(JwtRegisteredClaimNames.Email, userName),
                new(JwtRegisteredClaimNames.Name, userEmail)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]!));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = credentials,
                Issuer = _configuration["JwtService:Issuer"],
                Audience = _configuration["JwtService:Audience"]
            };

            return new JsonWebTokenHandler().CreateToken(tokenDescriptor);
        }
    }
}
