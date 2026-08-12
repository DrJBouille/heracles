using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Heracles.Application.Services;
using Heracles.Application.Services.Auth;
using Heracles.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Heracles.Infrastructure.Services;

public class TokenGenerator(IConfiguration configuration) : ITokenGenerator
{
    public string GenerateToken(ApplicationUser user, IList<string> roles)
    {
        var jwtSettings = configuration.GetSection("Jwt");

        var claims = new List<Claim>()
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.UserName!),
        };
        
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"]));
        var creadentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creadentials
        );
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}