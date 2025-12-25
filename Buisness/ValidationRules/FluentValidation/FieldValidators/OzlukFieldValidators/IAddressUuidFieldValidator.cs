using Business.ValidationRules.Commons;
using Core.Utilities.Validation;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation.FieldValidators.OzlukFieldValidators
{
    public interface IAddressUuidFieldValidator : IFieldValidator
    {
        Guid? AddressUuid { get; }
    }

    public class AddressUuidFieldValidator<T> : AbstractValidator<T>
        where T : IAddressUuidFieldValidator
    {
        public AddressUuidFieldValidator()
        {
            RuleFor(x => x.AddressUuid)
                .Cascade(CascadeMode.Stop)
                .NotNull().NotEmpty()
                    .WithMessage(Core.Utilities.Messages.ValidationMessageUtility.RequiredFormat(nameof(IAddressUuidFieldValidator.AddressUuid)))
                .Must(ValidationHelper.BeAValidUuid)
                    .WithMessage(Core.Utilities.Messages.ValidationMessageUtility.InvalidFormat(nameof(IAddressUuidFieldValidator.AddressUuid)));

        }
    }
}
