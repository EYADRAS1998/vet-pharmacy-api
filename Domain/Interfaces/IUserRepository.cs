using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUserRepository
    {
        // CRUD أساسية
        Task<User> GetById(int id);
        Task<IEnumerable<User>> GetAll();
        Task<bool> Add(User user);
        Task<bool> Update(User user);
        Task<bool> Delete(int id);

        // دوال إضافية مناسبة
       Task<User>GetByUsername(string username);
       Task<bool> Exists(string email);
    }
}
