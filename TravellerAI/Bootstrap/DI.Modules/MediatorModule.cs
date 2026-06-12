using Autofac;
using FluentValidation;
using MediatR;
using Optimove.OptiCustomersService.WebHost.Bootstrap.MediatR.Pipelines;
using System.Reflection;
using TravellerAI.Core.Features.AddBookingCommand;

namespace Optimove.OptiCustomersService.WebHost.Bootstrap.DI.Modules;

public class MediatorModule : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
            .AsImplementedInterfaces();

        var domainAssembly = typeof(AddBookingCommand).GetTypeInfo().Assembly;

        // Register all the Command classes (they implement IRequestHandler) in assembly holding the Commands
        builder.RegisterAssemblyTypes(domainAssembly)
            .AsClosedTypesOf(typeof(IRequestHandler<,>));

        builder.Register<ServiceFactory>(context =>
        {
            var componentContext = context.Resolve<IComponentContext>();
            return t => { object o; return componentContext.TryResolve(t, out o) ? o : null; };
        });

        builder.RegisterGeneric(typeof(LoggingBehavior<,>)).As(typeof(IPipelineBehavior<,>));
        builder.RegisterGeneric(typeof(ValidatorBehavior<,>)).As(typeof(IPipelineBehavior<,>));

        // register all validators for current domain
        builder.RegisterAssemblyTypes(domainAssembly)
            .Where(t => typeof(IValidator).IsAssignableFrom(t))
            .AsImplementedInterfaces();

    }
}