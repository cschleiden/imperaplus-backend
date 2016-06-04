using Autofac;
using ImperaPlus.Domain.Events;
using ImperaPlus.Domain.Services;

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
            builder.RegisterType<ScoringService>().As<IScoringService>();

            builder.RegisterType<EventAggregator>().As<IEventAggregator>().InstancePerRequest();
        }
    }
}
