using Autofac;
using AutofacSerilogIntegration;
using Microsoft.Extensions.Hosting;
using TravellerAI.Mapping;
using TravellerAI.Settings;

namespace Optimove.OptiCustomersService.WebHost.Bootstrap.DI.Modules;

public class ApplicationBootstrapperModule : Module
{
    private readonly AppSettings _appSettings;
    public ApplicationBootstrapperModule(AppSettings appSettings)
    {
        _appSettings = appSettings;
    }

    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterLogger();

        builder.RegisterInstance(_appSettings)
            .AsImplementedInterfaces()
            .AsSelf()
            .SingleInstance();
        
        builder.RegisterModule(
            new AutomapperModule(
                new MappingViewProfile(),
                new MappingEntitiesProfile()
            ));

        builder.RegisterModule(new ConfigurationModule());
        builder.RegisterModule(new InfrastructureModule());
        builder.RegisterModule(new MediatorModule());
    }
}