using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;


namespace luckyoneApiv3.Helper
{
    public class Jwt_Helper
    {

        private readonly IConfiguration _configuration;

        public Jwt_Helper(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public string GenrateJwtToken(int UserID, string role) 
        {
            var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, UserID.ToString()),           // JWT subject
                    new Claim(ClaimTypes.Role, role=="Admin" ? "Admin" : "User")         // Role
                };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


            var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(_configuration["Jwt:ExpiresInMinutes"])),
                    signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(token);


        }
    }
}
