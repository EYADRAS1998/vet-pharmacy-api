using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        // إضافة مستخدم جديد
        public async Task<bool> Add(User user)
        {
            await _context.Users.AddAsync(user);
            return await _context.SaveChangesAsync() > 0;
        }

        // حذف مستخدم
        public async Task<bool> Delete(int id)
        {
            var affected = await _context.Users
                .Where(u => u.Id == id)
                .ExecuteDeleteAsync();

            return affected > 0;
        }

        // التحقق من وجود بريد إلكتروني
        public async Task<bool> Exists(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }


        // جلب جميع المستخدمين
        public async Task<IEnumerable<User>> GetAll()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        // جلب مستخدم حسب Id
        public async Task<User> GetById(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        // جلب مستخدم حسب اسم المستخدم
        public async Task<User> GetByUsername(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<bool> Update(User user)
        {
            var affected = await _context.Users
       .Where(u => u.Id == user.Id)
       .ExecuteUpdateAsync(u => u
           .SetProperty(x => x.Username, user.Username)
           .SetProperty(x => x.Email, user.Email)
           .SetProperty(x => x.PasswordHash, user.PasswordHash)
           .SetProperty(x => x.Role, user.Role)
           .SetProperty(x => x.IsActive, user.IsActive));

            return affected > 0;

        }
    }
}
