using ChaatyApi.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ChaatyApi.Services
{
    public class TokenServices
    {
        private readonly SymmetricSecurityKey _key;
        private readonly IConfiguration config;

        public TokenServices(IConfiguration config)
        {
            _key = new SymmetricSecurityKey( Encoding.UTF8.GetBytes( config["JWT:Secret"]));
            this.config = config;
        }

        public string GenerateToken(AppUser user)
        {
            var claims = new List<Claim>
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: config["JWT:ValidIssuer"],
                audience: config["JWT:ValidAudience"],
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
