using Core.Utilities.Validation;

namespace Business.ValidationRules.FluentValidation.FieldValidators
{
    public interface IUserTypeFieldValidator : IFieldValidator
    {
        string? UserType { get; set; }
    }
}
