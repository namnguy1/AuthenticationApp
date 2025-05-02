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
            var record = await _repo.GetTokenRecordAsync(request.Token);
            if (record == null || record.Expiry < DateTime.UtcNow)
                throw new Exception("Token không hợp lệ hoặc đã hết hạn");

            var user = record.User!;
            user.IsActive = true;
            await _repo.UpdateAsync(user);

            // Xóa token sau khi dùng
            await _repo.RemoveVerificationTokenAsync(record);

            return "Xác thực tài khoản thành công";
        }
    }
}