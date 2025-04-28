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
    public class UserRepositor : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepositor(AppDbContext context)
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
    }
}