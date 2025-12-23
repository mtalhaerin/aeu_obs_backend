using Business.Features.CQRS.Auth.Login;
using Business.ValidationRules.FluentValidation.FieldValidators;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ValidationRules.FluentValidation
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            Include(new EmailFieldValidator<LoginCommand>());
            Include(new PasswordFieldValidator<LoginCommand>());
        }
    }
}
