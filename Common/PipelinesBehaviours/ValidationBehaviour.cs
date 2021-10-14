using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentResults;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace Common.PipelinesBehaviours
{
    public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : new()

    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            var response = new TResponse();
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);
                var validationResults =
                    await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
                var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();
                if (failures.Count > 0)
                {
                    if (response is ResultBase res)
                    {
                        var errors = GetResultErrors(failures);
                        foreach (var error in errors)
                        {
                            res.Reasons.Add(error);
                        }
                        
                        return response;
                    }
                    throw new ValidationException(failures);
                }
            }

            return await next();
        }
        private static IEnumerable<Error> GetResultErrors(List<ValidationFailure> failures)
        {
            var errors = failures.Select(e => new Error(e.ErrorMessage));
            
            return errors;
        }
    }
}