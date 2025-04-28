using Google.Apis.Auth;
using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NutritionApp.Application.Commands;
using NutritionApp.Application.Settings;
using NutritionApp.Domain.Entities;
using NutritionApp.Domain.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NutritionApp.Application.Handlers;

public class GoogleLoginCommandHandler : IRequestHandler<GoogleLoginCommand, string>
{
    private readonly IUserRepository _repo;
    private readonly JwtSettings _jwtSettings;

    public GoogleLoginCommandHandler(IUserRepository repo, IOptions<JwtSettings> jwtOpt)
    {
        _repo = repo;
        _jwtSettings = jwtOpt.Value;
    }

    public async Task<string> Handle(GoogleLoginCommand request, CancellationToken ct)
    {
        // Validate Google token
        var payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken);
        var user = await _repo.GetByEmailAsync(payload.Email!);
        if (user == null)
        {
            user = new User
            {
                Id = Guid.NewGuid(),
                Email = payload.Email!,
                IsActive = true,
                PasswordHash = string.Empty
            };
            await _repo.AddAsync(user);
        }

        // Tạo JWT tương tự như LoginQueryHandler
        var claims = new[] {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email)
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}