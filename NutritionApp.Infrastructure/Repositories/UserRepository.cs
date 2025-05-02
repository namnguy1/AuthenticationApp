using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NutritionApp.Domain.Entities;
using NutritionApp.Domain.Interfaces;
using NutritionApp.Infrastructure.Persistence;

namespace NutritionApp.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _context.Users.FindAsync(id);
        }
        // NutritionApp.Infrastructure/Repositories/UserRepository.cs
        public async Task AddVerificationTokenAsync(EmailVerificationToken token)
        {
            await _context.EmailVerificationTokens.AddAsync(token);
            await _context.SaveChangesAsync();
        }

        public async Task<EmailVerificationToken?> GetTokenRecordAsync(string token)
        {
    
            // Truy vấn từ database
            var tokenRecord = await _context.EmailVerificationTokens
                                            .Include(x => x.User)
                                            .FirstOrDefaultAsync(x => x.Token == token);
            // Ghi log kết quả truy vấn
            if (tokenRecord != null)
            {
                Console.WriteLine($"[DEBUG] Token found. ID: {tokenRecord.Id}, Email: {tokenRecord.User?.Email}, Used: {tokenRecord.IsUsed}");
            }
            else
            {
                Console.WriteLine("[DEBUG] Token not found in database.");
            }

            return tokenRecord;
        }


        public async Task RemoveVerificationTokenAsync(EmailVerificationToken token)
        {
            _context.EmailVerificationTokens.Remove(token);
            await _context.SaveChangesAsync();
        }

    }
}