using Business.ValidationRules.Commons;
using Core.Utilities.Messages;
using Core.Utilities.Validation;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ValidationRules.FluentValidation.FieldValidators.OzlukFieldValidators
{

    public interface IEmailUuidFieldValidator : IFieldValidator
    {
        Guid? EmailUuid { get; }
    }

    public class EmailUuidFieldValidator<T> : AbstractValidator<T>
        where T : IEmailUuidFieldValidator
    {
        public EmailUuidFieldValidator()
        {
            RuleFor(x => x.EmailUuid)
                .Cascade(CascadeMode.Stop)
                .NotNull().NotEmpty()
                    .WithMessage(ValidationMessageUtility.RequiredFormat(nameof(IEmailUuidFieldValidator.EmailUuid)))
                .Must(ValidationHelper.BeAValidUuid)
                    .WithMessage(ValidationMessageUtility.InvalidFormat(nameof(IEmailUuidFieldValidator.EmailUuid)));

        }
    }
}