using MediatR;
using Microsoft.Extensions.Options;
using NutritionApp.Application.Queries;
using NutritionApp.Application.Settings;
using NutritionApp.Domain.Interfaces;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace NutritionApp.Application.Handlers;

public class LoginQueryHandler : IRequestHandler<LoginQuery, string>
{
    private readonly IUserRepository _repo;
    private readonly JwtSettings _jwtSettings;

    public LoginQueryHandler(IUserRepository repo, IOptions<JwtSettings> jwtOpt)
    {
        _repo = repo;
        _jwtSettings = jwtOpt.Value;
    }

    public async Task<string> Handle(LoginQuery request, CancellationToken ct)
    {
        var user = await _repo.GetByEmailAsync(request.Email);
        if (user == null || !user.IsActive)
            throw new Exception("Email hoặc mật khẩu không đúng, hoặc tài khoản chưa xác thực");

        // TODO: Kiểm tra hash password. Demo bỏ qua.

        // Tạo JWT token
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