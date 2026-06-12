using Autofac;
using AutoMapper;

namespace Optimove.OptiCustomersService.WebHost.Bootstrap.DI.Modules;

public class AutomapperModule : Module
{
    private readonly Profile[] _externalProfiles;

    public AutomapperModule(params Profile[] externalProfiles)
    {
        _externalProfiles = externalProfiles;
    }

    protected override void Load(ContainerBuilder builder)
    {
        var mapperConfiguration = new MapperConfiguration(cfg =>
        {
            if (_externalProfiles != null)
            {
                foreach (var profile in _externalProfiles)
                {
                    cfg.AddProfile(profile);
                }
            }
        }, new LoggerFactory());

        var mapper = mapperConfiguration.CreateMapper();

        builder.RegisterInstance(mapper)
            .As<IMapper>()
            .SingleInstance();
    }
}