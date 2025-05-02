using MediatR;
using Microsoft.Extensions.Options;
using NutritionApp.Application.Commands;
using NutritionApp.Application.Settings;
using NutritionApp.Domain.Entities;
using NutritionApp.Domain.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace NutritionApp.Application.Handlers
{
    public class SignUpCommandHandler : IRequestHandler<SignUpCommand, string>
    {
        private readonly IUserRepository _repo;
        private readonly IEmailSender _emailSender;
        private readonly JwtSettings _jwtSettings;

        public SignUpCommandHandler(IUserRepository repo, IEmailSender emailSender, IOptions<JwtSettings> jwtOpt)
        {
            _repo = repo;
            _emailSender = emailSender;
            _jwtSettings = jwtOpt.Value;
        }

        public async Task<string> Handle(SignUpCommand request, CancellationToken ct)
        {
            var existing = await _repo.GetByEmailAsync(request.Email);
            if (existing != null) throw new Exception("Email đã tồn tại");

            // Hash password
            var hash = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)); // demo

            var user = new User { Id = Guid.NewGuid(), Email = request.Email, PasswordHash = hash, IsActive = false };
            await _repo.AddAsync(user);

            // Generate & save verification token
            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            var expiry = DateTime.UtcNow.AddHours(24);
            var tokenEntity = new EmailVerificationToken
            {
                UserId = user.Id,
                Token = token,
                Expiry = expiry
            };
            await _repo.AddVerificationTokenAsync(tokenEntity);

            // Gửi email
            var verifyUrl = $"http://localhost:5139/api/auth/verify-email?token={token}";
            var html = $"<p>Click <a href='{verifyUrl}'>vào đây</a> để xác thực tài khoản.</p>";
            await _emailSender.SendEmailAsync(user.Email, "Xác thực tài khoản", html);

            return "Vui lòng kiểm tra email để xác thực";
        }
    }
}