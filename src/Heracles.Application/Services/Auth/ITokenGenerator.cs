using Heracles.Domain.Entities;

namespace Heracles.Application.Services.Auth;

public interface ITokenGenerator
{
    string GenerateToken(ApplicationUser user, IList<string> roles);
}