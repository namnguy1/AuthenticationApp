using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using NutritionApp.Application.Commands;
using NutritionApp.Domain.Interfaces;

namespace NutritionApp.Application.Handlers
{
    public class VerifyEmailCommandHandler : IRequestHandler<VerifyEmailCommand, string>
    {
        private readonly IUserRepository _repo;
        public VerifyEmailCommandHandler(IUserRepository repo)
        {
            _repo = repo;
        }
        public async Task<string> Handle(VerifyEmailCommand request, CancellationToken ct)
        {
            // TODO: Lấy user qua token (từ DB hoặc cache). Ví dụ demo:
            var user = await _repo.GetByEmailAsync("email@demo.com");
            if (user == null) throw new Exception("Token không hợp lệ hoặc đã hết hạn");

            user.IsActive = true;
            await _repo.UpdateAsync(user);

            return "Xác thực tài khoản thành công";
        }
    }
}