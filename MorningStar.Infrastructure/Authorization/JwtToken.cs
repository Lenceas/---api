using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MorningStar.Infrastructure
{
    public static class JwtToken
    {
        /// <summary>
        /// 生成JwtToken
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GenerateJwtToken(long uid, string name)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppSettings.Get("Jwt:SecretKey")));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim("uid",uid.ToString()),
                new Claim(ClaimTypes.Name, name),
            };
            var token = new JwtSecurityToken(
                AppSettings.Get("Jwt:Issuer"),
                AppSettings.Get("Jwt:Audience"),
                claims,
                expires: DateTime.Now.AddMinutes(Convert.ToInt32(AppSettings.Get("Jwt:ExpiryInMinutes"))),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
