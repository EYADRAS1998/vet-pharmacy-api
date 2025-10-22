using Application.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;
using Application.Services.Interfaces;
using Application.DTOs.UserDTOs;
namespace Application.Services.implementation
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public UserService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<User?> GetByIdAsync(int id)
        {
           return await _userRepository.GetById(id);

        }

        // تسجيل دخول المستخدم
        public async Task<AuthResponseDto> LoginAsync(LoginUserDto dto)
        {
            // البحث عن المستخدم حسب البريد أو اسم المستخدم
            var user = await _userRepository
                .GetByUsername(dto.EmailOrUsername)
                ?? await _userRepository.GetByEmailAsync(dto.EmailOrUsername);

            if (user == null)
                throw new Exception("المستخدم غير موجود.");

            // التحقق من كلمة المرور
            bool validPassword = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
            if (!validPassword)
                throw new Exception("كلمة المرور غير صحيحة.");

            // إنشاء JWT token
            string token = GenerateJwtToken(user);

            return new AuthResponseDto
            {
                Token = token,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role
            };
        }

        // إنشاء حساب جديد
        public async Task<bool> RegisterAsync(RegisterUserDto dto)
        {
            // التحقق من وجود المستخدم مسبقًا
            var exists = await _userRepository.Exists(dto.Email);
            if (exists)
                throw new Exception("هذا البريد مستخدم بالفعل.");

            // تشفير كلمة المرور
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = hashedPassword,
                Role = dto.Role,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            return await _userRepository.Add(user);
        }

        public async Task<bool> UpdateAsync(UpdateUserDto dto)
        {
            var user = await _userRepository.GetById(dto.Id);
            if (user == null)
                return false;

            user.Username = dto.Username;
            user.Email = dto.Email;
            user.IsActive = dto.IsActive;

            return await _userRepository.Update(user);
        }

        public async Task<bool> UserExistsAsync(string email)
        {
            return await _userRepository.Exists(email);
        }
        private string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
