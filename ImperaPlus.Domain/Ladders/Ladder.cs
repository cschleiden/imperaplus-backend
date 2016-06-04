﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ImperaPlus.Domain.Games;
using ImperaPlus.Domain.Utilities;
using ImperaPlus.Domain.Exceptions;
using ImperaPlus.Domain.Annotations;

namespace ImperaPlus.Domain.Ladders
{
    public class Ladder
    {
        protected Ladder()
        {
            this.Id = Guid.NewGuid();           
            
            this.MapTemplates = new MapTemplateList();

            this.Standings = new HashSet<LadderStanding>();
            this.Queue = new HashSet<LadderQueueEntry>();
            this.Games = new HashSet<Game>();
        }

        public Ladder(string name, int numberOfTeams, int playersPerTeam)
            : this()
        {
            this.Name = name;

            this.Options = new GameOptions();
            this.Options.NumberOfTeams = numberOfTeams;
            this.Options.NumberOfPlayersPerTeam = playersPerTeam;
        }

        public Guid Id { get; private set; }

        public string Name { get; set; }

        public bool IsActive { get; private set; }

        /// <summary>
        /// Used for concurrency checks
        /// </summary>
        [Timestamp]
        public byte[] RowVersion { get; set; }

        public Games.GameOptions Options { get; private set; }

        public MapTemplateList MapTemplates { get; private set; }

        public virtual ICollection<LadderStanding> Standings { get; private set; }

        public virtual ICollection<LadderQueueEntry> Queue { get; private set; }

        public virtual ICollection<Game> Games { get; set; }

        public void QueueUser(User user)
        {
            // Ensure user is not already queue
            if (this.Queue.Any(x => x.UserId == user.Id))
            {
                throw new DomainException(ErrorCode.LadderUserAlreadyQueue, "User already queued for ladder");
            }

            var queueEntry = new LadderQueueEntry(this, user);
            this.Queue.Add(queueEntry);
        }

        public string GetGameName()
        {
            return string.Format("{0}-{1}", this.Name, DateTime.Now.Ticks);
        }

        public string GetMapTemplateForGame()
        {
            if (!this.MapTemplates.Any())
            {
                throw new DomainException(ErrorCode.LadderNoMapTemplates, "No map templates set for ladder");
            }

            return this.MapTemplates.Shuffle().First();
        }

        public void ToggleActive(bool isActive)
        {
            if (isActive)
            {
                if (this.MapTemplates.Count() == 0)
                {
                    throw new DomainException(ErrorCode.LadderCannotActivate, "MapTemplates are required for ladder to be active");
                }
            }

            this.IsActive = isActive;
        }
    }

    public class LadderQueueEntry : IChangeTrackedEntity
    {
        private Ladder ladder;

        [UsedImplicitly]
        protected LadderQueueEntry()
        {
        }

        public LadderQueueEntry(Ladder ladder, User user)
        {
            this.ladder = ladder;

            this.User = user;
            this.UserId = user.Id;
        }

        public Guid LadderId { get; set; }

        public string UserId { get; set; }
        public virtual User User { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime LastModifiedAt { get; set; }
    }

    public class LadderStanding
    {
        [UsedImplicitly]
        protected LadderStanding()
        {
        }

        public LadderStanding(Ladder ladder, User user)
        {
            this.LadderId = ladder.Id;
            this.UserId = user.Id;
        }

        public Guid LadderId { get; set; }

        public string UserId { get; set; }

        public virtual User User { get; set; }

        public int GamesPlayed { get; set; }

        public int GamesWon { get; set; }

        public int GamesLost { get; set; }

        public DateTime LastGame { get; set; }

        public double Rating { get; set; }

        public double Vol { get; set; }

        public double Rd { get; set; }
    }
}
