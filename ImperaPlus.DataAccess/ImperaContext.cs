using Autofac;
using ImperaPlus.Domain;
using ImperaPlus.Domain.Chat;
using ImperaPlus.Domain.Events;
using ImperaPlus.Domain.Games;
using ImperaPlus.Domain.Games.History;
using ImperaPlus.Domain.Ladders;
using ImperaPlus.Domain.Map;
using ImperaPlus.Domain.News;
using ImperaPlus.Domain.Tournaments;
using ImperaPlus.Domain.Utilities;
using Microsoft.AspNet.Identity.EntityFramework;
using StackExchange.Profiling;
using System;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace ImperaPlus.DataAccess
{
    public class ImperaContext : IdentityDbContext<User>, IImperaContext
    {
        private readonly IUserProvider userProvider;
        private readonly IComponentContext componentContext;
        private readonly IEventAggregator eventAggregator;

        public ImperaContext()
        {
            // Default constructor used for generating migrations
        }

        public ImperaContext(
            IUserProvider userProvider,
            IComponentContext componentContext,
            IEventAggregator eventAggregator)
        {
            this.userProvider = userProvider;
            this.componentContext = componentContext;
            this.eventAggregator = eventAggregator;

            ((IObjectContextAdapter)this).ObjectContext.ObjectMaterialized += ObjectContextOnObjectMaterialized;
        }

        /// <summary>
        /// Inject dependencies into domain objects
        /// </summary>
        private void ObjectContextOnObjectMaterialized(object sender, ObjectMaterializedEventArgs objectMaterializedEventArgs)
        {
            var entity = objectMaterializedEventArgs.Entity;

            if (this.componentContext != null)
            {
                // Satisfy required properties in 
                this.componentContext.InjectProperties(entity);
            }
        }

        public virtual IDbSet<Game> Games { get; set; }

        public virtual IDbSet<MapTemplateDescriptor> MapTemplates { get; set; }

        public virtual IDbSet<Channel> Channels { get; set; }

        public virtual IDbSet<ChatMessage> ChatMessages { get; set; }

        public virtual IDbSet<NewsEntry> NewsEntries { get; set; }

        public virtual IDbSet<Ladder> Ladders { get; set; }

        public virtual IDbSet<GameOptions> GameOptions { get; set; }

        public override int SaveChanges()
        {
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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            this.Configuration.LazyLoadingEnabled = true;
            this.Configuration.ProxyCreationEnabled = true;

            // Games
            modelBuilder.Entity<Game>().Ignore(x => x.MapTemplateProvider);
            modelBuilder.Entity<Game>().Ignore(x => x.AttackService);
            modelBuilder.Entity<Game>().Ignore(x => x.RandomGen);
            modelBuilder.Entity<Game>().Ignore(x => x.Map);
            modelBuilder.Entity<Game>().Ignore(x => x.CurrentPlayer);

            modelBuilder.Entity<Game>().HasMany(x => x.HistoryEntries).WithRequired().HasForeignKey(x => x.GameId).WillCascadeOnDelete();
            modelBuilder.Entity<Game>().HasMany(x => x.Teams).WithRequired(x => x.Game).HasForeignKey(x => x.GameId).WillCascadeOnDelete();
            modelBuilder.Entity<Team>().HasMany(x => x.Players).WithRequired(x => x.Team).HasForeignKey(x => x.TeamId).WillCascadeOnDelete();

            modelBuilder.Entity<HistoryEntry>().HasOptional(x => x.Actor).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<HistoryEntry>().HasOptional(x => x.OtherPlayer).WithMany().WillCascadeOnDelete(false);          
            
            // 
            modelBuilder.Entity<Channel>().HasOptional(x => x.Game);
            modelBuilder.Entity<Channel>().HasOptional(x => x.Alliance).WithRequired(x => x.Channel);
            modelBuilder.Entity<Channel>().HasMany(x => x.Messages).WithRequired(x => x.Channel).WillCascadeOnDelete();

            // Map Template
            modelBuilder.Entity<Continent>().HasMany(x => x.Countries).WithMany();
             
            // Country is serialized manually
            modelBuilder.Ignore<Country>();
             
            modelBuilder.Entity<LadderStanding>().HasKey(x => new { x.LadderId, x.UserId });
            modelBuilder.Entity<LadderQueueEntry>().HasKey(x => new { x.LadderId, x.UserId });


            modelBuilder.Entity<NewsEntry>().HasMany(x => x.Content).WithRequired().WillCascadeOnDelete();

            // Aliiance mapping
            modelBuilder.Entity<Alliance>().HasMany(x => x.Members).WithOptional(x => x.Alliance).HasForeignKey(x => x.AllianceId);
            modelBuilder.Entity<Alliance>().HasRequired(x => x.Channel).WithOptional().WillCascadeOnDelete();

            // Tournaments
            modelBuilder.Entity<Tournament>().HasMany(x => x.Teams).WithRequired(x => x.Tournament).HasForeignKey(x => x.TournamentId);
            modelBuilder.Entity<Tournament>().HasMany(x => x.Groups).WithRequired(x => x.Tournament).HasForeignKey(x => x.TournamentId);
            modelBuilder.Entity<Tournament>().HasMany(x => x.Pairings).WithRequired(x => x.Tournament).HasForeignKey(x => x.TournamentId);

            modelBuilder.Entity<TournamentTeam>().HasMany(x => x.Participants).WithRequired(x => x.Team).HasForeignKey(x => x.TeamId).WillCascadeOnDelete(false);

            modelBuilder.Entity<TournamentPairing>().HasMany(x => x.Games).WithOptional().HasForeignKey(x => x.TournamentPairingId);

            modelBuilder.Entity<TournamentPairing>().HasRequired(x => x.TeamA).WithMany().HasForeignKey(x => x.TeamAId).WillCascadeOnDelete(false);
            modelBuilder.Entity<TournamentPairing>().HasRequired(x => x.TeamB).WithMany().HasForeignKey(x => x.TeamBId).WillCascadeOnDelete(false);

            // General
            modelBuilder.ComplexType<CountryCollection>()
                .Ignore(x => x.Capacity);
            modelBuilder.ComplexType<CountryCollection>()
                .Property(x => x.Serialized).HasColumnName("CountriesJson");

            modelBuilder.ComplexType<MapTemplateList>()
                .Ignore(x => x.Capacity);
            modelBuilder.ComplexType<MapTemplateList>()
                .Property(x => x.Serialized).HasColumnName("MapTemplates");

            modelBuilder.ComplexType<VisibilityModifierCollection>()
                .Ignore(x => x.Capacity);
            modelBuilder.ComplexType<VisibilityModifierCollection>()
                .Property(x => x.Serialized).HasColumnName("VisibilityModifier");

            modelBuilder.ComplexType<VictoryConditionCollection>()
                .Ignore(x => x.Capacity);
            modelBuilder.ComplexType<VictoryConditionCollection>()
                .Property(x => x.Serialized).HasColumnName("VictoryConditions");
           
            modelBuilder.Entity<Domain.Messages.Message>().HasRequired(x => x.Owner).WithMany().WillCascadeOnDelete();
            modelBuilder.Entity<Domain.Messages.Message>().HasRequired(x => x.From).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<Domain.Messages.Message>().HasRequired(x => x.Recipient).WithMany().WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}