using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Platform.Infrastructure.Persistence;
using Platform.Infrastructure.Security;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Platform.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly PlatformDbContext _db;
        private readonly IConfiguration _config;

        public AuthController(PlatformDbContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Models.LoginRequest req)
        {
            var user = await _db.Users
                .FirstOrDefaultAsync(u => u.Username == req.Usuario && u.IsActive);

            if (user == null || !PasswordHasher.Verify(req.Contrasena, user.PasswordHash))
                return Unauthorized(new { message = "Credenciales inválidas" });

            var roles = await _db.UserRoles
                .Where(ur => ur.UserId == user.Id)
                .Join(_db.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r.Name)
                .ToListAsync();

            var modules = await _db.UserRoles
                .Where(ur => ur.UserId == user.Id)
                .Join(_db.RoleModules, ur => ur.RoleId, rm => rm.RoleId, (ur, rm) => rm.ModuleId)
                .Distinct()
                .Join(_db.Modules, mid => mid, m => m.Id, (mid, m) => m)
                .Where(m => m.IsActive)
                .OrderBy(m => m.Order)
                .Select(m => new
                {
                    code = m.Code,
                    name = m.Name,
                    description = m.Description,
                    icon = m.Icon,
                    route = m.Route
                })
                .ToListAsync();

            var token = GenerateJwt(user, roles);

            return Ok(new Models.LoginResponse
            {
                AccessToken = token,
                User = new Models.LoginUser
                {
                    Id = user.Id,
                    Usuario = user.Username,
                    Nombre = $"{user.FirstName} {user.LastName}".Trim(),
                    Roles = roles
                },
                Modules = modules
            });
        }

        private string GenerateJwt(Platform.Domain.Security.Entities.User user, List<string> roles)
        {
            var jwt = _config.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!));

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
                new Claim("name", $"{user.FirstName} {user.LastName}".Trim())
            };

            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwt["Issuer"],
                audience: jwt["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}