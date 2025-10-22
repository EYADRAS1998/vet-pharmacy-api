using Application.DTOs.UserDTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validation.UserValidation
{
    public class LoginUserValidator : AbstractValidator<LoginUserDto>
    {
        public LoginUserValidator()
        {
            RuleFor(x => x.EmailOrUsername)
                .NotEmpty().WithMessage("يرجى إدخال البريد الإلكتروني أو اسم المستخدم.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("يرجى إدخال كلمة المرور.");
        }
    }
}
