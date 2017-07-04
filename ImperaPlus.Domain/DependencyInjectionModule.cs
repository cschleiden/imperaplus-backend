using Autofac;
using ImperaPlus.Domain.Events;
using ImperaPlus.Domain.Services;
using ImperaPlus.Domain.Users;

namespace ImperaPlus.Domain
{
    public class DependencyInjectionModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RandomGen>().As<IRandomGen>();
            builder.RegisterType<AttackerRandomGen>().As<IAttackRandomGen>();
            builder.RegisterType<AttackService>().As<IAttackService>();

            builder.RegisterType<GameService>().As<IGameService>();
            builder.RegisterType<UserService>().As<IUserService>();
            builder.RegisterType<ScoringService>().As<IScoringService>();

            builder.RegisterType<EventAggregator>().As<IEventAggregator>().InstancePerLifetimeScope();

            // Notifications
            builder.RegisterType<Tournaments.EventHandler.AccountDeletedHandler>().As<IEventHandler<AccountDeleted>>();
            builder.RegisterType<Games.EventHandler.AccountDeletedHandler>().As<IEventHandler<AccountDeleted>>();
        }
    }
}
