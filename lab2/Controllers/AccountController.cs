using lab2.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace lab2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        [HttpPost]
       public IActionResult Login(userData test)
        {
            if(test.userName=="hema"&&test.password== "123")
            {
                List<Claim> claims = new List<Claim>();
                claims.Add(new Claim("Name", test.userName)); 
                claims.Add(new Claim(ClaimTypes.Email, "hema@ex.com"));

                string keyData = "hello from the other side a7aaaaaaaa";
                var secretKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(keyData));

                var signCredet = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    claims:claims,
                    expires:DateTime.Now.AddDays(1),
                    signingCredentials:signCredet
                    
                    );

                var stringToken = new JwtSecurityTokenHandler().WriteToken(token);
                return(Ok(stringToken));
                
            }

            return Unauthorized();
        }
    }
}
