using MediatR;
using Serilog;
using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using TravellerAI.Bootstrap.MediatR;
using TravellerAI.Core.Exceptions;
using TravellerAI.Domain.Exceptions;
using ILogger = Serilog.ILogger;

namespace Optimove.OptiCustomersService.WebHost.Bootstrap.MediatR.Pipelines;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly ILogger _logger;

    public LoggingBehavior(ILogger logger)
    {
        _logger = logger.ForContext<LoggingBehavior<TRequest, TResponse>>();
    }


    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _logger.Information("----- Handling command {CommandName} ({@Command})", request.GetType().GetGenericTypeName(), GetObjectProperties(request));
        try
        {
            var response = await next();
            _logger.Information("----- Command {CommandName} handled - response: {@Response}", request.GetGenericTypeName(), GetObjectProperties(response));

            return response;
        }
        catch (Exception ex) when (IsExpected(ex))
        {
            _logger.Warning("----- Command {CommandName} rejected: {ExceptionType} {Message}", request.GetGenericTypeName(), ex.GetType().Name, ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "----- Command {CommandName} failed", request.GetGenericTypeName());
            throw;
        }
    }

    /// <summary>
    /// Business / validation exceptions are expected outcomes and are logged without stack trace.
    /// </summary>
    private static bool IsExpected(Exception ex) =>
        ex is ValidationException or BadRequestException or UnauthorizedException or ForbiddenException
            or ConflictException or ResourceNotFoundException;

    /// <summary>
    /// Credentials and tokens are never written to logs.
    /// </summary>
    private static bool IsSensitive(string propertyName) =>
        propertyName.Contains("Password", StringComparison.OrdinalIgnoreCase)
        || propertyName.Contains("Token", StringComparison.OrdinalIgnoreCase);

    private string GetObjectProperties(object obj)
    {
        if (obj is ICollection)
        {
            var count = ((ICollection)obj).Count;
            return $"{obj.GetType()}: with {count} elements";
        }
        else if (obj is IEnumerable)
        {
            return "can't log IEnumerable types";
        }
        if (obj is ValueType)
        {
            return $"{obj.GetType()}:{obj}";
        }
        else
        {
            string props = string.Empty;
            if (obj != null)
            {
                var propsInfo = obj.GetType().GetProperties();
                for (int i = 0; i < propsInfo.Length; i++, props += ",")
                {
                    if (IsSensitive(propsInfo[i].Name))
                    {
                        props += $"{propsInfo[i].Name}:***";
                    }
                    else if (propsInfo[i].GetValue(obj) is ICollection)
                    {
                        var count = ((ICollection)propsInfo[i].GetValue(obj))!.Count;
                        props += $"{propsInfo[i].Name}: a collection with {count} elements";
                    }
                    else
                    {
                        props += $"{propsInfo[i].Name}:{propsInfo[i].GetValue(obj)}";
                    }
                }
            }
            return props;
        }
    }
}