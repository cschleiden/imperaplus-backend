using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using ImperaPlus.DataAccess;
using ImperaPlus.Domain.Alliances;
using ImperaPlus.Domain.Enums;
using ImperaPlus.Domain.Games.History;
using ImperaPlus.Domain.Messages;
using ImperaPlus.Domain.Tournaments;

namespace ImperaPlus.Web.Migrations
{
    [DbContext(typeof(ImperaContext))]
    [Migration("20180411042956_FixChannelAllianceMapping2")]
    partial class FixChannelAllianceMapping2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ImperaPlus.Domain.Alliances.Alliance", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("ChannelId");

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("ChannelId");

                    b.ToTable("Alliances");
                });

            modelBuilder.Entity("ImperaPlus.Domain.Alliances.AllianceJoinRequest", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("AllianceId");

                    b.Property<string>("ApprovedByUserId");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("DeniedByUserId");

                    b.Property<DateTime>("LastModifiedAt");

                    b.Property<string>("Reason");

                    b.Property<string>("RequestedByUserId");

                    b.Property<int>("State");

                    b.HasKey("Id");

                    b.HasIndex("AllianceId");

                    b.HasIndex("ApprovedByUserId");

                    b.HasIndex("DeniedByUserId");

                    b.HasIndex("RequestedByUserId");

                    b.ToTable("AllianceJoinRequest");
                });

            modelBuilder.Entity("ImperaPlus.Domain.Chat.Channel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("GameId");

                    b.Property<string>("Name");

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.HasIndex("GameId")
                        .IsUnique();

                    b.ToTable("Channels");
                });

            modelBuilder.Entity("ImperaPlus.Domain.Chat.ChatMessage", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("ChannelId");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("CreatedById");

                    b.Property<DateTime>("LastModifiedAt");

                    b.Property<string>("Text");

                    b.HasKey("Id");

                    b.HasIndex("ChannelId");

                    b.HasIndex("CreatedById");

                    b.ToTable("ChatMessages");
                });

            modelBuilder.Entity("ImperaPlus.Domain.Games.Chat.GameChatMessage", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateTime");

                    b.Property<long>("GameId");

                    b.Property<Guid?>("TeamId");

                    b.Property<string>("Text");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.HasIndex("TeamId");

                    b.HasIndex("UserId");

                    b.ToTable("GameChatMessage");
                });

            modelBuilder.Entity("ImperaPlus.Domain.Games.Game", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AttacksInCurrentTurn");

                    b.Property<bool>("CardDistributed");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("CreatedById");

                    b.Property<Guid?>("CurrentPlayerId");

                    b.Property<Guid?>("LadderId");

                    b.Property<bool?>("LadderScored");

                    b.Property<DateTime>("LastModifiedAt");

                    b.Property<DateTime>("LastTurnStartedAt");

                    b.Property<string>("MapTemplateName");

                    b.Property<int>("MovesInCurrentTurn");

                    b.Property<string>("Name");

                    b.Property<long>("OptionsId");

                    b.Property<string>("Password");

                    b.Property<int>("PlayState");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<string>("SerializedCountries");

                    b.Property<DateTime?>("StartedAt");

                    b.Property<int>("State");

                    b.Property<Guid?>("TournamentPairingId");

                    b.Property<int>("TurnCounter");

                    b.Property<int>("Type");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.HasIndex("LadderId");

                    b.HasIndex("OptionsId");

                    b.HasIndex("TournamentPairingId");

                    b.HasIndex("UserId");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("ImperaPlus.Domain.Games.GameOptions", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AttacksPerTurn");

                    b.Property<int>("InitialCountryUnits");

                    b.Property<int>("MapDistribution");

                    b.Property<int>("MaximumNumberOfCards");

                    b.Property<int>("MaximumTimeoutsPerPlayer");

                    b.Property<int>("MinUnitsPerCountry");

                    b.Property<int>("MovesPerTurn");

                    b.Property<int>("NewUnitsPerTurn");

                    b.Property<int>("NumberOfPlayersPerTeam");

                    b.Property<int>("NumberOfTeams");

                    b.Property<string>("SerializedVictoryConditions");

                    b.Property<string>("SerializedVisibilityModifier");

                    b.Property<int>("TimeoutInSeconds");

                    b.HasKey("Id");

                    b.ToTable("GameOptions");
                });

            modelBuilder.Entity("ImperaPlus.Domain.Games.History.HistoryEntry", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Action");

                    b.Property<Guid?>("ActorId");

                    b.Property<DateTime>("DateTime");

                    b.Property<string>("DestinationIdentifier");

                    b.Property<long>("GameId");

                    b.Property<string>("OriginIdentifier");

                    b.Property<Guid?>("OtherPlayerId");

                    b.Property<bool?>("Result");

                    b.Property<long>("TurnNo");

                    b.Property<int?>("Units");

                    b.Property<int?>("UnitsLost");

                    b.Property<int?>("UnitsLostOther");

                    b.HasKey("Id");

                    b.HasIndex("ActorId");

                    b.HasIndex("GameId");

                    b.HasIndex("OtherPlayerId");

                    b.ToTable("HistoryEntry");
                });

            modelBuilder.Entity("ImperaPlus.Domain.Games.Player", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Bonus");

                    b.Property<long?>("GameId");

                    b.Property<string>("InternalCardData");

                    b.Property<bool>("IsHidden");

                    b.Property<int>("Outcome");

                    b.Property<bool>("PlacedInitialUnits");

                    b.Property<int>("PlayOrder");

                    b.Property<int>("State");

                    b.Property<Guid>("TeamId");

                    b.Property<int>("Timeouts");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("TeamId");

                    b.HasIndex("UserId");

                    b.HasIndex("GameId", "UserId")
                        .IsUnique();

                    b.ToTable("Player");
                });

            modelBuilder.Entity("ImperaPlus.Domain.Games.Team", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("GameId");

                    b.Property<int>("PlayOrder");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.ToTable("Team");
                });

            modelBuilder.Entity("ImperaPlus.Domain.Ladders.Ladder", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsActive");

                    b.Property<string>("Name");

                    b.Property<long?>("OptionsId");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<string>("SerializedMapTemplates");

                    b.HasKey("Id");

                    b.HasIndex("OptionsId");

                    b.ToTable("Ladders");
                });

            modelBuilder.Entity("ImperaPlus.Domain.Ladders.LadderQueueEntry", b =>
                {
                    b.Property<Guid>("LadderId");

                    b.Property<string>("UserId");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<DateTime>("LastModifiedAt");

                    b.HasKey("LadderId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("LadderQueueEntry");
                });

            modelBuilder.Entity("ImperaPlus.Domain.Ladders.LadderStanding", b =>
                {
                    b.Property<Guid>("LadderId");

                    b.Property<string>("UserId");

                    b.Property<int>("GamesLost");

                    b.Property<int>("GamesPlayed");

                    b.Property<int>("GamesWon");

                    b.Property<DateTime>("LastGame");

                    b.Property<double>("Rating");

                    b.Property<double>("Rd");

                    b.Property<string>("UserId1");

                    b.Property<double>("Vol");

                    b.HasKey("LadderId", "UserId");

                    b.HasIndex("UserId");

                    b.HasIndex("UserId1");

                    b.ToTable("LadderStanding");
                });

            modelBuilder.Entity("ImperaPlus.Domain.Map.MapTemplateDescriptor", b =>
                {
                    b.Property<string>("Name")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("CreatedBy");

                    b.Property<bool>("IsActive");

                    b.Property<DateTime>("LastModifiedAt");

                    b.HasKey("Name");

                    b.ToTable("MapTemplates");
                });

            modelBuilder.Entity("ImperaPlus.Domain.Messages.Message", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<int>("Folder");

                    b.Property<string>("FromId");

                    b.Property<bool>("IsRead");

                    b.Property<DateTime>("LastModifiedAt");

                    b.Property<string>("OwnerId")
                        .IsRequired();

                    b.Property<string>("RecipientId");

                    b.Property<string>("Subject");

                    b.Property<string>("Text");

                    b.HasKey("Id");

                    b.HasIndex("FromId");

                    b.HasIndex("OwnerId");

                    b.HasIndex("RecipientId");

                    b.ToTable("Message");
                });

            modelBuilder.Entity("ImperaPlus.Domain.News.NewsContent", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Language");

                    b.Property<long?>("NewsEntryId")
                        .IsRequired();

                    b.Property<string>("Text");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.HasIndex("NewsEntryId");

                    b.ToTable("NewsContent");
                });

            modelBuilder.Entity("ImperaPlus.Domain.News.NewsEntry", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("CreatedById");

                    b.Property<DateTime>("LastModifiedAt");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.ToTable("NewsEntries");
                });

            modelBuilder.Entity("ImperaPlus.Domain.Tournaments.Tournament", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<DateTime?>("EndOfTournament");

                    b.Property<DateTime>("LastModifiedAt");

                    b.Property<string>("Name");

                    b.Property<int>("NumberOfFinalGames");

                    b.Property<int>("NumberOfGroupGames");

                    b.Property<int>("NumberOfKnockoutGames");

                    b.Property<int>("NumberOfTeams");

                    b.Property<long>("OptionsId");

                    b.Property<int>("Phase");

                    b.Property<string>("SerializedMapTemplates");

                    b.Property<DateTime>("StartOfRegistration");

                    b.Property<DateTime>("StartOfTournament");

                    b.Property<int>("State");

                    b.Property<Guid?>("WinnerId");

                    b.HasKey("Id");

                    b.HasIndex("OptionsId");

                    b.HasIndex("WinnerId")
                        .IsUnique();

                    b.ToTable("Tournament");
                });

            modelBuilder.Entity("ImperaPlus.Domain.Tournaments.TournamentGroup", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Number");

                    b.Property<Guid>("TournamentId");

                    b.Property<Guid?>("TournamentId1");

                    b.HasKey("Id");

                    b.HasIndex("TournamentId");

                    b.HasIndex("TournamentId1");

                    b.ToTable("TournamentGroup");
                });

            modelBuilder.Entity("ImperaPlus.Domain.Tournaments.TournamentPairing", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("GroupId");

                    b.Property<int>("NumberOfGames");

                    b.Property<int>("Order");

                    b.Property<int>("Phase");

                    b.Property<int>("State");

                    b.Property<Guid>("TeamAId");

                    b.Property<int>("TeamAWon");

                    b.Property<Guid>("TeamBId");

                    b.Property<int>("TeamBWon");

                    b.Property<Guid?>("TournamentGroupId");

                    b.Property<Guid>("TournamentId");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.HasIndex("TeamAId");

                    b.HasIndex("TeamBId");

                    b.HasIndex("TournamentGroupId");

                    b.HasIndex("TournamentId");

                    b.ToTable("TournamentPairing");
                });

            modelBuilder.Entity("ImperaPlus.Domain.Tournaments.TournamentParticipant", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("TeamId");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("TeamId");

                    b.HasIndex("UserId");

                    b.ToTable("TournamentParticipant");
                });

            modelBuilder.Entity("ImperaPlus.Domain.Tournaments.TournamentTeam", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CreatedById");

                    b.Property<Guid?>("GroupId");

                    b.Property<int>("GroupOrder");

                    b.Property<string>("Name");

                    b.Property<string>("Password");

                    b.Property<int>("State");

                    b.Property<Guid>("TournamentId");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.HasIndex("GroupId");

                    b.HasIndex("TournamentId");

                    b.ToTable("TournamentTeam");
                });

            modelBuilder.Entity("ImperaPlus.Domain.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<Guid?>("AllianceId");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<int>("GameSlots");

                    b.Property<bool>("IsAllianceAdmin");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Language");

                    b.Property<DateTime>("LastLogin");

                    b.Property<string>("LegacyPasswordHash");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("AllianceId");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("OpenIddict.Models.OpenIddictApplication", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClientId");

                    b.Property<string>("ClientSecret");

                    b.Property<string>("DisplayName");

                    b.Property<string>("LogoutRedirectUri");

                    b.Property<string>("RedirectUri");

                    b.Property<string>("Type");

                    b.HasKey("Id");

                    b.HasIndex("ClientId")
                        .IsUnique();

                    b.ToTable("OpenIddictApplications");
                });

            modelBuilder.Entity("OpenIddict.Models.OpenIddictAuthorization", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ApplicationId");

                    b.Property<string>("Scope");

                    b.Property<string>("Subject");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationId");

                    b.ToTable("OpenIddictAuthorizations");
                });

            modelBuilder.Entity("OpenIddict.Models.OpenIddictScope", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.HasKey("Id");

                    b.ToTable("OpenIddictScopes");
                });

            modelBuilder.Entity("OpenIddict.Models.OpenIddictToken", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ApplicationId");

                    b.Property<string>("AuthorizationId");

                    b.Property<string>("Subject");

                    b.Property<string>("Type");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationId");

                    b.HasIndex("AuthorizationId");

                    b.ToTable("OpenIddictTokens");
                });

            modelBuilder.Entity("ImperaPlus.Domain.Alliances.Alliance", b =>
                {
                    b.HasOne("ImperaPlus.Domain.Chat.Channel", "Channel")
                        .WithMany()
                        .HasForeignKey("ChannelId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ImperaPlus.Domain.Alliances.AllianceJoinRequest", b =>
                {
                    b.HasOne("ImperaPlus.Domain.Alliances.Alliance", "Alliance")
                        .WithMany("Requests")
                        .HasForeignKey("AllianceId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ImperaPlus.Domain.User", "ApprovedByUser")
                        .WithMany()
                        .HasForeignKey("ApprovedByUserId");

                    b.HasOne("ImperaPlus.Domain.User", "DeniedByUser")
                        .WithMany()
                        .HasForeignKey("DeniedByUserId");

                    b.HasOne("ImperaPlus.Domain.User", "RequestedByUser")
                        .WithMany()
                        .HasForeignKey("RequestedByUserId");
                });

            modelBuilder.Entity("ImperaPlus.Domain.Chat.Channel", b =>
                {
                    b.HasOne("ImperaPlus.Domain.Games.Game", "Game")
                        .WithOne()
                        .HasForeignKey("ImperaPlus.Domain.Chat.Channel", "GameId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ImperaPlus.Domain.Chat.ChatMessage", b =>
                {
                    b.HasOne("ImperaPlus.Domain.Chat.Channel", "Channel")
                        .WithMany("Messages")
                        .HasForeignKey("ChannelId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ImperaPlus.Domain.User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById");
                });

            modelBuilder.Entity("ImperaPlus.Domain.Games.Chat.GameChatMessage", b =>
                {
                    b.HasOne("ImperaPlus.Domain.Games.Game")
                        .WithMany("ChatMessages")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ImperaPlus.Domain.Games.Team", "Team")
                        .WithMany()
                        .HasForeignKey("TeamId");

                    b.HasOne("ImperaPlus.Domain.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.SetNull);
                });

            modelBuilder.Entity("ImperaPlus.Domain.Games.Game", b =>
                {
                    b.HasOne("ImperaPlus.Domain.User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("ImperaPlus.Domain.Ladders.Ladder", "Ladder")
                        .WithMany("Games")
                        .HasForeignKey("LadderId");

                    b.HasOne("ImperaPlus.Domain.Games.GameOptions", "Options")
                        .WithMany()
                        .HasForeignKey("OptionsId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ImperaPlus.Domain.Tournaments.TournamentPairing")
                        .WithMany("Games")
                        .HasForeignKey("TournamentPairingId");

                    b.HasOne("ImperaPlus.Domain.User")
                        .WithMany("CreatedGames")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("ImperaPlus.Domain.Games.History.HistoryEntry", b =>
                {
                    b.HasOne("ImperaPlus.Domain.Games.Player", "Actor")
                        .WithMany()
                        .HasForeignKey("ActorId");

                    b.HasOne("ImperaPlus.Domain.Games.Game", "Game")
                        .WithMany("HistoryEntries")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ImperaPlus.Domain.Games.Player", "OtherPlayer")
                        .WithMany()
                        .HasForeignKey("OtherPlayerId");
                });

            modelBuilder.Entity("ImperaPlus.Domain.Games.Player", b =>
                {
                    b.HasOne("ImperaPlus.Domain.Games.Game", "Game")
                        .WithMany()
                        .HasForeignKey("GameId");

                    b.HasOne("ImperaPlus.Domain.Games.Team", "Team")
                        .WithMany("Players")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ImperaPlus.Domain.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.SetNull);
                });

            modelBuilder.Entity("ImperaPlus.Domain.Games.Team", b =>
                {
                    b.HasOne("ImperaPlus.Domain.Games.Game", "Game")
                        .WithMany("Teams")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ImperaPlus.Domain.Ladders.Ladder", b =>
                {
                    b.HasOne("ImperaPlus.Domain.Games.GameOptions", "Options")
                        .WithMany()
                        .HasForeignKey("OptionsId");
                });

            modelBuilder.Entity("ImperaPlus.Domain.Ladders.LadderQueueEntry", b =>
                {
                    b.HasOne("ImperaPlus.Domain.Ladders.Ladder")
                        .WithMany("Queue")
                        .HasForeignKey("LadderId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ImperaPlus.Domain.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ImperaPlus.Domain.Ladders.LadderStanding", b =>
                {
                    b.HasOne("ImperaPlus.Domain.Ladders.Ladder")
                        .WithMany("Standings")
                        .HasForeignKey("LadderId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ImperaPlus.Domain.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ImperaPlus.Domain.User")
                        .WithMany("Standings")
                        .HasForeignKey("UserId1");
                });

            modelBuilder.Entity("ImperaPlus.Domain.Messages.Message", b =>
                {
                    b.HasOne("ImperaPlus.Domain.User", "From")
                        .WithMany()
                        .HasForeignKey("FromId");

                    b.HasOne("ImperaPlus.Domain.User", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ImperaPlus.Domain.User", "Recipient")
                        .WithMany()
                        .HasForeignKey("RecipientId");
                });

            modelBuilder.Entity("ImperaPlus.Domain.News.NewsContent", b =>
                {
                    b.HasOne("ImperaPlus.Domain.News.NewsEntry")
                        .WithMany("Content")
                        .HasForeignKey("NewsEntryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ImperaPlus.Domain.News.NewsEntry", b =>
                {
                    b.HasOne("ImperaPlus.Domain.User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById");
                });

            modelBuilder.Entity("ImperaPlus.Domain.Tournaments.Tournament", b =>
                {
                    b.HasOne("ImperaPlus.Domain.Games.GameOptions", "Options")
                        .WithMany()
                        .HasForeignKey("OptionsId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ImperaPlus.Domain.Tournaments.TournamentTeam", "Winner")
                        .WithOne()
                        .HasForeignKey("ImperaPlus.Domain.Tournaments.Tournament", "WinnerId");
                });

            modelBuilder.Entity("ImperaPlus.Domain.Tournaments.TournamentGroup", b =>
                {
                    b.HasOne("ImperaPlus.Domain.Tournaments.Tournament", "Tournament")
                        .WithMany()
                        .HasForeignKey("TournamentId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ImperaPlus.Domain.Tournaments.Tournament")
                        .WithMany("Groups")
                        .HasForeignKey("TournamentId1");
                });

            modelBuilder.Entity("ImperaPlus.Domain.Tournaments.TournamentPairing", b =>
                {
                    b.HasOne("ImperaPlus.Domain.Tournaments.TournamentGroup", "Group")
                        .WithMany()
                        .HasForeignKey("GroupId");

                    b.HasOne("ImperaPlus.Domain.Tournaments.TournamentTeam", "TeamA")
                        .WithMany()
                        .HasForeignKey("TeamAId");

                    b.HasOne("ImperaPlus.Domain.Tournaments.TournamentTeam", "TeamB")
                        .WithMany()
                        .HasForeignKey("TeamBId");

                    b.HasOne("ImperaPlus.Domain.Tournaments.TournamentGroup")
                        .WithMany("Pairings")
                        .HasForeignKey("TournamentGroupId");

                    b.HasOne("ImperaPlus.Domain.Tournaments.Tournament", "Tournament")
                        .WithMany("Pairings")
                        .HasForeignKey("TournamentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ImperaPlus.Domain.Tournaments.TournamentParticipant", b =>
                {
                    b.HasOne("ImperaPlus.Domain.Tournaments.TournamentTeam", "Team")
                        .WithMany("Participants")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ImperaPlus.Domain.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.SetNull);
                });

            modelBuilder.Entity("ImperaPlus.Domain.Tournaments.TournamentTeam", b =>
                {
                    b.HasOne("ImperaPlus.Domain.User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById");

                    b.HasOne("ImperaPlus.Domain.Tournaments.TournamentGroup", "Group")
                        .WithMany("Teams")
                        .HasForeignKey("GroupId");

                    b.HasOne("ImperaPlus.Domain.Tournaments.Tournament", "Tournament")
                        .WithMany("Teams")
                        .HasForeignKey("TournamentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ImperaPlus.Domain.User", b =>
                {
                    b.HasOne("ImperaPlus.Domain.Alliances.Alliance", "Alliance")
                        .WithMany("Members")
                        .HasForeignKey("AllianceId")
                        .OnDelete(DeleteBehavior.SetNull);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Claims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("ImperaPlus.Domain.User")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("ImperaPlus.Domain.User")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ImperaPlus.Domain.User")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("OpenIddict.Models.OpenIddictAuthorization", b =>
                {
                    b.HasOne("OpenIddict.Models.OpenIddictApplication", "Application")
                        .WithMany("Authorizations")
                        .HasForeignKey("ApplicationId");
                });

            modelBuilder.Entity("OpenIddict.Models.OpenIddictToken", b =>
                {
                    b.HasOne("OpenIddict.Models.OpenIddictApplication", "Application")
                        .WithMany("Tokens")
                        .HasForeignKey("ApplicationId");

                    b.HasOne("OpenIddict.Models.OpenIddictAuthorization", "Authorization")
                        .WithMany("Tokens")
                        .HasForeignKey("AuthorizationId");
                });
        }
    }
}
