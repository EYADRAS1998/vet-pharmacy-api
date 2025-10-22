using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class User
    {
        public int Id { get; set; }              // Primary key
        public string Username { get; set; }     // اسم المستخدم
        public string Email { get; set; }        // البريد الإلكتروني
        public string PasswordHash { get; set; } // كلمة المرور مشفرة
        public UserRoleEnum Role { get; set; } = UserRoleEnum.User;
        // Admin / Employee / Customer
        public bool IsActive { get; set; } // لتحديد إذا كان المستخدم نشطًا
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        // علاقات مع كيانات أخرى (اختياري)
        // public ICollection<Order> Orders { get; set; }
    }
}
