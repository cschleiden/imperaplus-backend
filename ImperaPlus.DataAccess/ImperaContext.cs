using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using ImperaPlus.Domain;
using ImperaPlus.Domain.Alliances;
using ImperaPlus.Domain.Chat;
using ImperaPlus.Domain.Events;
using ImperaPlus.Domain.Games;
using ImperaPlus.Domain.Games.Chat;
using ImperaPlus.Domain.Games.History;
using ImperaPlus.Domain.Ladders;
using ImperaPlus.Domain.Map;
using ImperaPlus.Domain.News;
using ImperaPlus.Domain.Tournaments;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StackExchange.Profiling;

namespace ImperaPlus.DataAccess
{
    public class ImperaContext : IdentityDbContext<User>, IImperaContext
    {
        private readonly Domain.IUserProvider userProvider;
        private readonly IEventAggregator eventAggregator;

        public ImperaContext(DbContextOptions<ImperaContext> options)
            : base(options)
        {
        }

        public ImperaContext(
            DbContextOptions<ImperaContext> options,
            Domain.IUserProvider userProvider,
            IEventAggregator eventAggregator)
            : base(options)
        {
            this.userProvider = userProvider;
            this.eventAggregator = eventAggregator;
        }

        public virtual DbSet<Alliance> Alliances { get; set; }

        public virtual DbSet<Game> Games { get; set; }

        public virtual DbSet<MapTemplateDescriptor> MapTemplates { get; set; }

        public virtual DbSet<Channel> Channels { get; set; }

        public virtual DbSet<ChatMessage> ChatMessages { get; set; }

        public virtual DbSet<NewsEntry> NewsEntries { get; set; }

        public virtual DbSet<Ladder> Ladders { get; set; }

        public virtual DbSet<GameOptions> GameOptions { get; set; }
        
        public override int SaveChanges()
        {
            var aggregatedEventQueue = new List<IDomainEvent>();

            using (MiniProfiler.Current.Step("Context: Update change tracked entitites"))
            {
                var changeTrackedEntities = this.ChangeTracker.Entries<IChangeTrackedEntity>().ToArray();

                foreach (var entry in changeTrackedEntities)
                {
                    if (entry.State == EntityState.Added && entry.Entity.CreatedAt == default(DateTime))
                    {
                        entry.Entity.CreatedAt = DateTime.UtcNow;
                    }

                    if (entry.State == EntityState.Modified || entry.State == EntityState.Added)
                    {
                        entry.Entity.LastModifiedAt = DateTime.UtcNow;
                    }
                }

                foreach (
                    var entry in
                        this.ChangeTracker.Entries<IOwnedEntity>().Where(x => x.State == EntityState.Added && x.Entity.CreatedBy == null).ToArray())
                {
                    entry.Entity.CreatedById = this.userProvider.GetCurrentUserId();
                }

                // Ensure all serialized collections are updated
                foreach (var entry in this.ChangeTracker.Entries<ISerializedEntity>())
                {
                    entry.Entity.Serialize();
                }

                // Aggregate events                
                foreach (var entry in this.ChangeTracker.Entries<Entity>())
                {
                    aggregatedEventQueue.AddRange(entry.Entity.EventQueue.Events);
                }                
            }

            if (this.eventAggregator != null)
            {
                foreach (var ev in aggregatedEventQueue)
                {
                    this.eventAggregator.Raise(ev);
                }
            }

            int result;
            using (MiniProfiler.Current.Step("Context: Save changes"))
            {
                result = base.SaveChanges();
            }

            using (MiniProfiler.Current.Step("Context: Handle Queued Events"))
            {
                if (this.eventAggregator != null)
                {
                    this.eventAggregator.HandleQueuedEvents();
                }
            }

            return result;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityUserLogin<string>>().HasKey(p => new { p.UserId, p.LoginProvider });

            modelBuilder.Entity<User>()
                .HasMany(e => e.Logins)
                .WithOne()
                .HasForeignKey(e => e.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Roles)
                .WithOne()
                .HasForeignKey(e => e.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Games            
            modelBuilder.Entity<Game>()
                .HasMany(x => x.HistoryEntries)
                .WithOne(x => x.Game)
                .IsRequired()
                .HasForeignKey(x => x.GameId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Game>()
                .HasMany(x => x.Teams)
                .WithOne(x => x.Game)
                .IsRequired()
                .HasForeignKey(x => x.GameId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Game>()
                .HasOne(x => x.CreatedBy)
                .WithMany()
                .HasForeignKey(x => x.CreatedById)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Team>()
                .HasMany(x => x.Players)
                .WithOne(x => x.Team)
                .IsRequired()
                .HasForeignKey(x => x.TeamId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Player>()
                .HasOne(x => x.User)
                .WithMany()
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Player>()
                .HasOne(x => x.Game)
                .WithMany()
                .HasForeignKey(x => x.GameId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Player>()
                .HasIndex(x => new { x.GameId, x.UserId })
                .IsUnique();

            // Game chat
            modelBuilder.Entity<GameChatMessage>()
                .HasOne(x => x.User)
                .WithMany()
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            // Game History
            modelBuilder.Entity<HistoryEntry>().HasOne(x => x.Actor).WithMany().IsRequired(false).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<HistoryEntry>().HasOne(x => x.OtherPlayer).WithMany().IsRequired(false).OnDelete(DeleteBehavior.Restrict);
            
            // Chat
            modelBuilder.Entity<Channel>()
                .HasOne(x => x.Game)
                .WithOne()
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Channel>()
                .HasMany(x => x.Messages)
                .WithOne(x => x.Channel)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Channel>()
                .Ignore(x => x.RecentMessages);

            // Countries are serialized manually
            modelBuilder.Ignore<Continent>();
            modelBuilder.Ignore<CountryTemplate>();
            modelBuilder.Ignore<Country>();
            modelBuilder.Ignore<Connection>();
            modelBuilder.Ignore<MapTemplate>();
             
            // Ladder
            modelBuilder.Entity<LadderStanding>().HasKey(x => new { x.LadderId, x.UserId });
            modelBuilder.Entity<LadderStanding>().HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<LadderQueueEntry>().HasKey(x => new { x.LadderId, x.UserId });
            modelBuilder.Entity<LadderQueueEntry>().HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<NewsEntry>().HasMany(x => x.Content).WithOne().IsRequired().OnDelete(DeleteBehavior.Cascade);

            // Aliiance mapping
            modelBuilder.Entity<Alliance>()
                .HasMany(x => x.Members)
                .WithOne(x => x.Alliance)
                .HasForeignKey(x => x.AllianceId)
                .OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<Alliance>()
                .HasMany(x => x.Requests)
                .WithOne(x => x.Alliance)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Alliance>()
                .HasOne(x => x.Channel)
                .WithMany()
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade)
                .HasForeignKey(x => x.ChannelId);

            // Tournaments           
            modelBuilder.Entity<Tournament>().HasMany(x => x.Teams).WithOne(x => x.Tournament).HasForeignKey(x => x.TournamentId).IsRequired().OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Tournament>().HasMany(x => x.Groups).WithOne(x => x.Tournament).HasForeignKey(x => x.TournamentId).IsRequired().OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Tournament>().HasMany(x => x.Pairings).WithOne(x => x.Tournament).HasForeignKey(x => x.TournamentId).IsRequired().OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Tournament>().HasOne(x => x.Winner).WithOne().IsRequired(false);

            modelBuilder.Entity<TournamentTeam>()
                .HasMany(x => x.Participants)
                .WithOne(x => x.Team)
                .HasForeignKey(x => x.TeamId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TournamentParticipant>()
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<TournamentPairing>().HasMany(x => x.Games).WithOne().IsRequired(false).HasForeignKey(x => x.TournamentPairingId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<TournamentPairing>().HasOne(x => x.TeamA).WithMany().IsRequired().HasForeignKey(x => x.TeamAId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<TournamentPairing>().HasOne(x => x.TeamB).WithMany().IsRequired().HasForeignKey(x => x.TeamBId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<TournamentPairing>().HasOne(x => x.Group).WithMany().IsRequired(false).HasForeignKey(x => x.GroupId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TournamentGroup>().HasMany(x => x.Teams).WithOne(x => x.Group).HasForeignKey(x => x.GroupId).IsRequired(false).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<TournamentGroup>().HasMany(x => x.Pairings).WithOne().IsRequired(false).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<TournamentGroup>().HasOne(x => x.Tournament).WithMany().IsRequired(true).OnDelete(DeleteBehavior.Cascade);

            // Messages
            modelBuilder.Entity<Domain.Messages.Message>()
                .HasOne(x => x.Owner)
                .WithMany()
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Domain.Messages.Message>()
                .HasOne(x => x.From)
                .WithMany()
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Domain.Messages.Message>()
                .HasOne(x => x.Recipient)
                .WithMany()
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}