using Autofac;
using TravellerAI.Core.Repositories;
using TravellerAI.Infrastructure.Db.Mssql.Repositories;

namespace Optimove.OptiCustomersService.WebHost.Bootstrap.DI.Modules;

public class InfrastructureModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        // TravellerDbContext itself is registered in ServiceBootstrapper.RegisterDatabase (per request scope)
        builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();

        builder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerLifetimeScope();
        builder.RegisterType<JourneyRepository>().As<IJourneyRepository>().InstancePerLifetimeScope();
        builder.RegisterType<TripRepository>().As<ITripRepository>().InstancePerLifetimeScope();
        builder.RegisterType<BookingRepository>().As<IBookingRepository>().InstancePerLifetimeScope();
        builder.RegisterType<TransportRepository>().As<ITransportRepository>().InstancePerLifetimeScope();
    }
}
