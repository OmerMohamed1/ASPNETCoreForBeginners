using ASPNETCoreForBeginners.Data;
using ASPNETCoreForBeginners.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.AccessControl;
using System.Security.Claims;
using System.Text;

namespace ASPNETCoreForBeginners.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class UsersController(JwtOptions jwtOptions, ApplicationDbContext dbContext) : ControllerBase
    {

        [HttpPost]
        [Route("Auth")]
        public ActionResult<string> AuthenticateUser(AuthenticationRequest request)
        {
            var user = dbContext.Set<User>().FirstOrDefault(x => x.Name == request.UserName &&
                                                             x.Password == request.Password);
            if (user == null)
            {
                return Unauthorized();
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescription = new SecurityTokenDescriptor
            {
                Issuer = jwtOptions.Issuer,
                Audience = jwtOptions.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey)),
                SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Role, "Admin"),
                    new Claim(ClaimTypes.Role, "SuperUser"),
                    new ("UserTypes", "Employee"),
                    new ("DateOfBirth", "2006-01-01"),
                })
            };

            var securityToken = tokenHandler.CreateToken(tokenDescription);
            var jwtToken = tokenHandler.WriteToken(securityToken);

            return Ok(jwtToken);
        }

    }
}










































// تشفير رمز JWT باستخدام JWE
//var encryptionKey = Convert.FromBase64String(jwtOptions.EncryptionKey);
//var encryptedToken = Jose.JWT.Encode(jwtToken, encryptionKey, JweAlgorithm.A256KW, JweEncryption.A256CBC_HS512);