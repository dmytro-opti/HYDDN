using FluentValidation;
using MediatR;
using TravellerAI.Bootstrap.MediatR;
using ILogger = Serilog.ILogger;
using ValidationException = FluentValidation.ValidationException;

namespace Optimove.OptiCustomersService.WebHost.Bootstrap.MediatR.Pipelines;

public class ValidatorBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly ILogger _logger;
    private readonly IValidator<TRequest>[] _validators;

    public ValidatorBehavior(IValidator<TRequest>[] validators, ILogger logger)
    {
        _validators = validators;
        _logger = logger.ForContext<ValidatorBehavior<TRequest, TResponse>>();
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next();
        }

        var typeName = request.GetGenericTypeName();

        _logger.Information("----- Validating command {CommandType}", typeName);

        var context = new ValidationContext<TRequest>(request);
        var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

        if (failures.Any())
        {
            _logger.Warning("Validation errors - {CommandType} - Command: {@Command} - Errors: {@ValidationErrors}", typeName, request, failures);

            throw new ValidationException($"Command Validation Errors for type {typeof(TRequest).Name}", failures);
        }

        return await next();
    }
}