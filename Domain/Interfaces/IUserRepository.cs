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
        User GetById(int id);
        IEnumerable<User> GetAll();
        void Add(User user);
        void Update(User user);
        void Delete(int id);

        // دوال إضافية مناسبة
        User GetByUsername(string username);
        bool Exists(string email);
    }
}
