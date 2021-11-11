using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ImperaPlus.Domain.Games;
using ImperaPlus.Domain.Utilities;
using ImperaPlus.Domain.Exceptions;
using System.ComponentModel.DataAnnotations.Schema;
using ImperaPlus.Domain.Services;

namespace ImperaPlus.Domain.Ladders
{
    public class Ladder
    {
        protected Ladder()
        {
            Id = Guid.NewGuid();

            MapTemplates = new SerializedCollection<string>();

            Standings = new HashSet<LadderStanding>();
            Queue = new HashSet<LadderQueueEntry>();
            Games = new HashSet<Game>();
        }

        public Ladder(string name, int numberOfTeams, int playersPerTeam)
            : this()
        {
            Name = name;

            Options = new GameOptions();
            Options.NumberOfTeams = numberOfTeams;
            Options.NumberOfPlayersPerTeam = playersPerTeam;
        }

        public string SerializedMapTemplates
        {
            get => MapTemplates.Serialize();

            set => MapTemplates = new SerializedCollection<string>(value);
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; private set; }

        public string Name { get; set; }

        public bool IsActive { get; private set; }

        /// <summary>
        /// Used for concurrency checks
        /// </summary>
        [Timestamp]
        public byte[] RowVersion { get; set; }

        public virtual GameOptions Options { get; private set; }

        [NotMapped] public SerializedCollection<string> MapTemplates { get; private set; }

        public virtual ICollection<LadderStanding> Standings { get; private set; }

        public virtual ICollection<LadderQueueEntry> Queue { get; private set; }

        public virtual ICollection<Game> Games { get; set; }

        public void QueueUser(User user)
        {
            // Ensure user is not already queue
            if (Queue.Any(x => x.UserId == user.Id))
            {
                throw new DomainException(ErrorCode.LadderUserAlreadyQueue, "User already queued for ladder");
            }

            var queueEntry = new LadderQueueEntry(this, user);
            Queue.Add(queueEntry);
        }

        public void QueueLeaveUser(User user)
        {
            // Ensure user is in queue
            var entryForUser = Queue.FirstOrDefault(x => x.UserId == user.Id);
            if (entryForUser == null)
            {
                throw new DomainException(ErrorCode.LadderUserNotInQueue, "User is not queued for ladder");
            }

            Queue.Remove(entryForUser);
        }

        public string GetGameName()
        {
            return string.Format("{0}-{1}", Name, DateTime.Now.Ticks);
        }

        public string GetMapTemplateForGame(IRandomGen random)
        {
            if (!MapTemplates.Any())
            {
                throw new DomainException(ErrorCode.LadderNoMapTemplates, "No map templates set for ladder");
            }

            return MapTemplates.Shuffle(random).First();
        }

        public void ToggleActive(bool isActive)
        {
            if (isActive)
            {
                if (MapTemplates.Count() == 0)
                {
                    throw new DomainException(ErrorCode.LadderCannotActivate,
                        "MapTemplates are required for ladder to be active");
                }
            }

            IsActive = isActive;
        }
    }
}
