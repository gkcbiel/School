using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using School.Data;
using School.Models;

namespace School.Controllers
{
    [Route("v1/auth")]
    public class UserController : Controller
    {
        private readonly JWTSettings _jwtSettings;
        private readonly DataContext _context;

        public UserController(IOptions<JWTSettings> jwtsettings, DataContext context)
        {
            _jwtSettings = jwtsettings.Value;
            _context = context;
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult<string> CreateUsers([FromBody] User model)
        {
            if (ModelState.IsValid)
            {
                if (!_context.User.Any(d => d.Email == model.Email && d.Password == model.Password))
                {
                    _context.User.Add(model);
                    int codReturn = _context.SaveChanges();

                    if (codReturn > 0)
                    {
                        return Ok("Usuário cadastrado com sucesso");
                    }
                    else
                    {
                        return BadRequest("Não foi possível cadastrar o usuário");
                    }
                }
                else
                {
                    return BadRequest("Usuário já existe");
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public ActionResult<UserToken> Authenticate([FromBody] User model)
        {
            var user = _context.User.FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);

            if (user == null)
            {
                return NotFound(new { message = "Email ou senha inválidos" });
            }
            else
            {
                var token = GenerateAccessToken(user);

                return new UserToken
                {
                    User = user,
                    Token = token
                };
            }
        }
        private string GenerateAccessToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]{
                    new Claim(ClaimTypes.Name, user.Email.ToString()),
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}