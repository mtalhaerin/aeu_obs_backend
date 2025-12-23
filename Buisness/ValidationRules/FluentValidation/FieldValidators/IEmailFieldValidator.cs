using Business.ValidationRules.Commons;
using Core.Utilities.Validation;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ValidationRules.FluentValidation.FieldValidators
{
    public interface IEmailFieldValidator : IFieldValidator 
    {
        string? Email { get; set; }
    }

    public class EmailFieldValidator<T> : AbstractValidator<T>
        where T : IEmailFieldValidator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmailFieldValidator{T}"/> class.
        /// Sets up validation rules for the <see cref="IEmailFieldValidator.Email"/> property.
        /// </summary>
        public EmailFieldValidator()
        {   
            RuleFor(x => x.Email)
                .Cascade(CascadeMode.Stop)
                .NotNull().NotEmpty()
                    .WithMessage(ValidationMessages.RequiredFormat(nameof(IEmailFieldValidator.Email)))

                .MaximumLength(100)
                    .WithMessage(ValidationMessages.MaxLengthFormat(nameof(IEmailFieldValidator.Email), 100))

                .EmailAddress()
                    .WithMessage(ValidationMessages.EmailFormat(nameof(IEmailFieldValidator.Email)))

                .Must(ValidationHelper.BeAValidEmail)
                    .WithMessage(ValidationMessages.NotAccepedEmailFormat(nameof(IEmailFieldValidator.Email)));
        }
    }
}
