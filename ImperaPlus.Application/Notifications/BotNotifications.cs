using Hangfire;
using ImperaPlus.Application.Jobs;
using ImperaPlus.Domain.Events;
using ImperaPlus.Domain.Games.Events;

namespace ImperaPlus.Application.Notifications
{
    /// <summary>
    /// Invoke the bot whenever a game is started or a turn has ended. Actual Bot invocation is performed out of process.
    /// </summary>
    public class BotNotifications : ICompletedEventHandler<TurnEndedEvent>, ICompletedEventHandler<GameStartedEvent>
    {
        private IBackgroundJobClient backgroundJobClient;

        public BotNotifications(IBackgroundJobClient backgroundJobClient)
        {
            this.backgroundJobClient = backgroundJobClient;
        }

        /// <summary>
        /// Handle start of game event (bot might be first player)
        /// </summary>
        /// <param name="evt"></param>
        public void Handle(GameStartedEvent evt)
        {
            this.HandleGameEvent(evt);
        }

        /// <summary>
        /// Handle end of turn event
        /// </summary>
        /// <param name="evt"></param>
        public void Handle(TurnEndedEvent evt)
        {
            this.HandleGameEvent(evt);
        }
                       
        private void HandleGameEvent(GameEvent evt)
        {
            if (evt.Game.State == Domain.Enums.GameState.Active
                && evt.Game.CurrentPlayer.User.UserName == Constants.BotName)
            {
                this.backgroundJobClient.Enqueue<BotJob>(x => x.Play(evt.Game.Id, null));
            }
        }
    }
}
