using Application.DTOs;
using Application.DTOs.UserDTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IUserService
    {
        // إنشاء حساب جديد
        Task<bool> RegisterAsync(RegisterUserDto dto);

        // تسجيل دخول المستخدم وإرجاع التوكن
        Task<AuthResponseDto> LoginAsync(LoginUserDto dto);

        // التحقق من وجود مستخدم ببريد إلكتروني معين
        Task<bool> UserExistsAsync(string email);

        // جلب مستخدم حسب المعرّف (اختياري)
        Task<User?> GetByIdAsync(int id);

        // تحديث بيانات المستخدم (اختياري)
        Task<bool> UpdateAsync(UpdateUserDto dto);
    }
}
