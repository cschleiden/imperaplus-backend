using System;

namespace ImperaPlus.Domain.Games.Chat
{
    public class GameChatMessage : IIdentifiableEntity
    {
        protected GameChatMessage()
        {
        }

        public GameChatMessage(Game game, User user, Team team, string text)
        {
            this.GameId = game.Id;
            this.User = user;
            this.Team = team;
            this.Text = text;
            this.DateTime = DateTime.UtcNow;
        }

        public long Id { get; set; }

        public long GameId { get; set; }

        public string UserId { get; set; }
        public virtual User User { get; set; }

        public Guid? TeamId { get; set; }
        public virtual Team Team { get; set; }

        public DateTime DateTime { get; set; }

        public string Text { get; set; }
    }
}
