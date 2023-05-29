using Microsoft.IdentityModel.Tokens;
using Pattern.DTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Pattern.Utility
{
    public class JwtHelper
    {
        public static string GenerateToken(JwtSetting jwtSetting, UserDTO user)
        {
            if (jwtSetting == null)
                return string.Empty;

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting.SecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.Actor, user.Position),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),

            };

            var authToken = new JwtSecurityToken(
                jwtSetting.Issuer,
                jwtSetting.Audience,
                claims,
                expires: DateTime.UtcNow.AddMinutes(jwtSetting.ExpiryMinutes), // Default 5 mins, max 1 day
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(authToken);
        }
    }
}