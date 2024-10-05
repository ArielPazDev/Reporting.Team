using Backend.API.RESTful.Models;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace Backend.API.RESTful.Services
{
    public class JwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(UserModel user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:key"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity([
                    new Claim(ClaimTypes.NameIdentifier, user.IDUser.ToString()),
                    new Claim(ClaimTypes.Name, user.NameFirst + " " + user.NameLast),
                    new Claim(ClaimTypes.Email, user.Email)
                    ]),
                Expires = DateTime.Now.AddMinutes(30),
                SigningCredentials = credentials
            };

            var token = new JsonWebTokenHandler().CreateToken(tokenDescriptor);

            return token;
        }
    }
}
