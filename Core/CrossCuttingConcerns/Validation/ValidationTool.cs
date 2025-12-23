using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CrossCuttingConcerns.Validation
{
    public static class ValidationTool
    {
        public static void Validate(IValidator validator, object? entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Entity to validate cannot be null.");
            }
            var context = new ValidationContext<object>(entity);
            // Context'i validate metoduna veriyoruz.
            var result = validator.Validate(context);

            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }
        }
    }
}