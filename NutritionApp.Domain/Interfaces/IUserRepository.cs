using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NutritionApp.Domain.Entities;

namespace NutritionApp.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task<User?> GetByIdAsync(Guid id);
        Task AddVerificationTokenAsync(EmailVerificationToken token);
        Task<EmailVerificationToken?> GetTokenRecordAsync(string token);
        Task RemoveVerificationTokenAsync(EmailVerificationToken token);
    }
}