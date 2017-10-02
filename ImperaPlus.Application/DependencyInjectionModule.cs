using Autofac;
using Hangfire;
using ImperaPlus.Application.Chat;
using ImperaPlus.Application.Games;
using ImperaPlus.Application.Jobs;
using ImperaPlus.Application.Ladder;
using ImperaPlus.Application.Messages;
using ImperaPlus.Application.News;
using ImperaPlus.Application.Notifications;
using ImperaPlus.Application.Play;
using ImperaPlus.Application.Tournaments;
using ImperaPlus.Application.Users;
using ImperaPlus.Application.Visibility;
using ImperaPlus.Domain.Games.Events;

namespace ImperaPlus.Application
{
    public class DependencyInjectionModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MapTemplateProvider>().As<Domain.Services.IMapTemplateProvider>().SingleInstance();        

            builder.RegisterType<GameService>().As<IGameService>();
            builder.RegisterType<PlayService>().As<IPlayService>();
            builder.RegisterType<MapTemplateService>().As<IMapTemplateService>();
            builder.RegisterType<ChatService>().As<IChatService>();
            builder.RegisterType<NewsService>().As<INewsService>();
            builder.RegisterType<LadderService>().As<ILadderService>();
            builder.RegisterType<MessageService>().As<IMessageService>();
            builder.RegisterType<UserService>().As<IUserService>();
            builder.RegisterType<NotificationService>().As<INotificationService>();
            builder.RegisterType<TournamentService>().As<ITournamentService>();

            // Notifications
            builder.RegisterType<PushNotifications>()
                .As<Domain.Events.ICompletedEventHandler<TurnEndedEvent>>()
                .As<Domain.Events.ICompletedEventHandler<PlayerSurrenderedEvent>>();

            builder.RegisterType<BotNotifications>()
                .As<Domain.Events.ICompletedEventHandler<GameStartedEvent>>()
                .As<Domain.Events.ICompletedEventHandler<TurnEndedEvent>>();

            builder.RegisterType<LadderNotifications>()
                .As<Domain.Events.ICompletedEventHandler<GameEndedEvent>>();
            
            // Jobs
            builder.RegisterType<TimeoutJob>().AsSelf();
            builder.RegisterType<LadderJob>().AsSelf();
            builder.RegisterType<LadderScorejob>().AsSelf();
            builder.RegisterType<BotJob>().AsSelf();
            builder.RegisterType<NotificationJob>().AsSelf();
            builder.RegisterType<UserCleanupJob>().AsSelf();

            builder.RegisterType<TournamentJob>().AsSelf();
            builder.RegisterType<TournamentStartJob>().AsSelf();


            builder.RegisterType<VisibilityModifierFactory>().AsImplementedInterfaces();

            builder.RegisterType<RandomGenProvider>().AsImplementedInterfaces();
            builder
                .Register(c => c.Resolve<IRandomGenProvider>().GetRandomGen())
                .As<Domain.Services.IRandomGen>()
                .InstancePerLifetimeScope();
        }
    }
}
