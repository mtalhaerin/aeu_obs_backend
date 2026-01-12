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
    public interface ITelefonUuidFieldValidator : IFieldValidator
    {
        Guid? TelefonUuid { get; }
    }

    public class TelefonUuidFieldValidator<T> : AbstractValidator<T>
        where T : ITelefonUuidFieldValidator
    {
        public TelefonUuidFieldValidator()
        {
            RuleFor(x => x.TelefonUuid)
                .Cascade(CascadeMode.Stop)
                .NotNull().NotEmpty()
                    .WithMessage(ValidationMessageUtility.RequiredFormat(nameof(ITelefonUuidFieldValidator.TelefonUuid)))
                .Must(ValidationHelper.BeAValidUuid)
                    .WithMessage(ValidationMessageUtility.InvalidFormat(nameof(ITelefonUuidFieldValidator.TelefonUuid)));

        }
    }
}
