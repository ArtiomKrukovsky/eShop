using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Ordering.Domain.Exceptions;

namespace Ordering.API.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse>
         : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators) => _validators = validators;

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            var failures = _validators
                .Select(v => v.Validate(request))
                .SelectMany(result => result.Errors)
                .Where(error => error != null)
                .ToList();

            if (failures.Any())
            {
                var errorFieldsMessages = failures.Select(x => x.ErrorMessage + ", ").ToArray();

                throw new OrderingDomainException(
                    $"Command Validation Errors for type {typeof(TRequest).Name}. " +
                            $"Validation failed : {string.Join(string.Empty, errorFieldsMessages)}", new ValidationException("Validation exception", failures));
            }

            var response = await next();
            return response;
        }
    }
}
