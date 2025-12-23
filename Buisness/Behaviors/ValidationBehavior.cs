using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        private readonly ILogger<ValidationBehavior<TRequest, TResponse>> _logger;

        public ValidationBehavior(
            IEnumerable<IValidator<TRequest>> validators,
            ILogger<ValidationBehavior<TRequest, TResponse>> logger)
        {
            _validators = validators;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (!_validators.Any())
            {
                return await next();
            }

            var requestName = typeof(TRequest).Name;
            _logger.LogInformation("Validation başladı: {RequestName}", requestName);

            var context = new ValidationContext<TRequest>(request);

            // DEĞİŞİKLİK BURADA: Task.WhenAll yerine foreach döngüsü
            // Validatorları tek tek gez, hata bulursan ANINDA dur.
            foreach (var validator in _validators)
            {
                var result = await validator.ValidateAsync(context, cancellationToken);

                if (!result.IsValid)
                {
                    var errors = result.Errors.Select(f => f.ErrorMessage).ToList();
                    _logger.LogWarning("Validation başarısız (İlk Hata): {RequestName} - Hata: {@Errors}", requestName, errors);

                    // İlk bulduğun hatada exception fırlat ve işlemi kes
                    throw new ValidationException(result.Errors);
                }
            }

            _logger.LogInformation("Validation başarılı: {RequestName}", requestName);
            return await next();
        }
    }
}