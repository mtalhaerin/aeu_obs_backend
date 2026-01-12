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
    public interface IKullaniciUuidFieldValidator : IFieldValidator
    {
        Guid? KullaniciUuid { get; }
    }

    public class KullaniciUuidFieldValidator<T> : AbstractValidator<T>
        where T : IKullaniciUuidFieldValidator
    {
        public KullaniciUuidFieldValidator()
        {
            RuleFor(x => x.KullaniciUuid)
                .Cascade(CascadeMode.Stop)
                .NotNull().NotEmpty()
                    .WithMessage(ValidationMessageUtility.RequiredFormat(nameof(IKullaniciUuidFieldValidator.KullaniciUuid)))
                .Must(ValidationHelper.BeAValidUuid)
                    .WithMessage(ValidationMessageUtility.InvalidFormat(nameof(IKullaniciUuidFieldValidator.KullaniciUuid)));

        }
    }
}
