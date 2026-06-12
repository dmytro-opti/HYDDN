using MediatR;
using Serilog;
using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using TravellerAI.Bootstrap.MediatR;
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
        var response = await next();
        _logger.Information("----- Command {CommandName} handled - response: {@Response}", request.GetGenericTypeName(), GetObjectProperties(response));

        return response;
    }

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
                    if (propsInfo[i].GetValue(obj) is ICollection)
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