using Autofac;
using ImperaPlus.Domain.Events;
using ImperaPlus.Domain.Services;

namespace ImperaPlus.Domain
{
    public class DependencyInjectionModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AttackerRandomGen>().As<IAttackRandomGen>();
            builder.RegisterType<AttackService>().As<IAttackService>();

            builder.RegisterType<GameService>().As<IGameService>();
            builder.RegisterType<UserService>().As<IUserService>();
            builder.RegisterType<ScoringService>().As<IScoringService>();

            builder.RegisterType<EventAggregator>().As<IEventAggregator>().InstancePerLifetimeScope();

            // Notifications
            builder.RegisterAssemblyTypes(typeof(DependencyInjectionModule).Assembly).AsClosedTypesOf(typeof(IEventHandler<>));

            //builder.RegisterType<Tournaments.EventHandler.AccountDeletedHandler>().As<IEventHandler<AccountDeleted>>();
            //builder.RegisterType<Games.EventHandler.AccountDeletedHandler>().As<IEventHandler<AccountDeleted>>();
            // builder.RegisterType<Messages.EventHandler.AccountDeletedHandler>().As<IEventHandler<AccountDeleted>>();
        }
    }
}
