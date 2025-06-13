using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace Poliedro.Eds.Application.Common.Behaviors;

public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_validators.Any())
        {
            ValidationContext<TRequest> context = new ValidationContext<TRequest>(request);
            List<ValidationFailure> list = (await Task.WhenAll(_validators.Select((v) => v.ValidateAsync(context, cancellationToken)))).Where((r) => r.Errors.Any()).SelectMany((r) => r.Errors).ToList();
            if (list.Any())
            {
                throw new ValidationException(list);
            }
        }

        return await next();
    }
}
