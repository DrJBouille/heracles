using Heracles.Application.Dtos.Auth;
using Heracles.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Heracles.Application.Services.Auth;

public class AuthService(UserManager<ApplicationUser> userManager, ITokenGenerator tokenGenerator) : IAuthService
{
    public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request)
    {
        var user = new ApplicationUser
        {
            Email = request.Email,
            UserName = request.Username
        };
        
        var result = await userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded) throw new InvalidOperationException(string.Join(", ", result.Errors.Select(e => e.Description)));
        
        await userManager.AddToRoleAsync(user, "User");
        
        var roles =  await userManager.GetRolesAsync(user);
        var token = tokenGenerator.GenerateToken(user, roles);
        
        return new AuthResponseDto(token, user.UserName);
    }

    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
    {
        var user = await userManager.FindByEmailAsync(request.Email) ?? throw new UnauthorizedAccessException("Username or password incorrect.");

        var isPasswordValid = await userManager.CheckPasswordAsync(user, request.Password);
        if (!isPasswordValid) throw new UnauthorizedAccessException("Username or password incorrect.");
        
        var roles =  await userManager.GetRolesAsync(user);
        var token = tokenGenerator.GenerateToken(user, roles);
        
        return new AuthResponseDto(token, user.UserName!);
    }
}