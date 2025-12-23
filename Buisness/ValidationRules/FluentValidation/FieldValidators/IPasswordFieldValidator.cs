using Business.ValidationRules.Commons;
using Core.Utilities.Validation;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation.FieldValidators
{
    public interface IPasswordFieldValidator : IFieldValidator
    {
        string? Password { get; set; }
    }

    public class PasswordFieldValidator<T> : AbstractValidator<T>
        where T : IPasswordFieldValidator
    {
        public PasswordFieldValidator()
        {
            RuleFor(x => x.Password)
                .Cascade(CascadeMode.Stop)
                .NotNull().NotEmpty()
                    .WithMessage(ValidationMessages.RequiredFormat(nameof(IPasswordFieldValidator.Password)))

                .MinimumLength(8)
                    .WithMessage(ValidationMessages.MinLengthFormat(nameof(IPasswordFieldValidator.Password), 8))

                .MaximumLength(100)
                    .WithMessage(ValidationMessages.MaxLengthFormat(nameof(IPasswordFieldValidator.Password), 8))

                .Must(ValidationHelper.BeAValidPassword)
                    .WithMessage(ValidationMessages.PasswordMustBeAValidFormat(nameof(IPasswordFieldValidator.Password)));
        }
    }
}
