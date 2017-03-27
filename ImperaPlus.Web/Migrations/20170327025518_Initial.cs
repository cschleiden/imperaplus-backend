using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ImperaPlus.Web.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GameOptions",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AttacksPerTurn = table.Column<int>(nullable: false),
                    InitialCountryUnits = table.Column<int>(nullable: false),
                    MapDistribution = table.Column<int>(nullable: false),
                    MaximumNumberOfCards = table.Column<int>(nullable: false),
                    MaximumTimeoutsPerPlayer = table.Column<int>(nullable: false),
                    MinUnitsPerCountry = table.Column<int>(nullable: false),
                    MovesPerTurn = table.Column<int>(nullable: false),
                    NewUnitsPerTurn = table.Column<int>(nullable: false),
                    NumberOfPlayersPerTeam = table.Column<int>(nullable: false),
                    NumberOfTeams = table.Column<int>(nullable: false),
                    SerializedVictoryConditions = table.Column<string>(nullable: true),
                    SerializedVisibilityModifier = table.Column<string>(nullable: true),
                    TimeoutInSeconds = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameOptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MapTemplates",
                columns: table => new
                {
                    Name = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    LastModifiedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MapTemplates", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                });

            migrationBuilder.CreateTable(
                name: "OpenIddictApplications",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ClientId = table.Column<string>(nullable: true),
                    ClientSecret = table.Column<string>(nullable: true),
                    DisplayName = table.Column<string>(nullable: true),
                    LogoutRedirectUri = table.Column<string>(nullable: true),
                    RedirectUri = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenIddictApplications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OpenIddictScopes",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenIddictScopes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ladders",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    OptionsId = table.Column<long>(nullable: true),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    SerializedMapTemplates = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ladders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ladders_GameOptions_OptionsId",
                        column: x => x.OptionsId,
                        principalTable: "GameOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OpenIddictAuthorizations",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ApplicationId = table.Column<string>(nullable: true),
                    Scope = table.Column<string>(nullable: true),
                    Subject = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenIddictAuthorizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OpenIddictAuthorizations_OpenIddictApplications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "OpenIddictApplications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OpenIddictTokens",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ApplicationId = table.Column<string>(nullable: true),
                    AuthorizationId = table.Column<string>(nullable: true),
                    Subject = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenIddictTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OpenIddictTokens_OpenIddictApplications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "OpenIddictApplications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OpenIddictTokens_OpenIddictAuthorizations_AuthorizationId",
                        column: x => x.AuthorizationId,
                        principalTable: "OpenIddictAuthorizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    AllianceId = table.Column<Guid>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    GameSlots = table.Column<int>(nullable: false),
                    IsAllianceAdmin = table.Column<bool>(nullable: false),
                    Language = table.Column<string>(nullable: true),
                    LegacyPasswordHash = table.Column<string>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    PasswordHash = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    SecurityStamp = table.Column<string>(nullable: true),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LadderQueueEntry",
                columns: table => new
                {
                    LadderId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    LastModifiedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LadderQueueEntry", x => new { x.LadderId, x.UserId });
                    table.ForeignKey(
                        name: "FK_LadderQueueEntry_Ladders_LadderId",
                        column: x => x.LadderId,
                        principalTable: "Ladders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LadderQueueEntry_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LadderStanding",
                columns: table => new
                {
                    LadderId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<string>(nullable: false),
                    GamesLost = table.Column<int>(nullable: false),
                    GamesPlayed = table.Column<int>(nullable: false),
                    GamesWon = table.Column<int>(nullable: false),
                    LastGame = table.Column<DateTime>(nullable: false),
                    Rating = table.Column<double>(nullable: false),
                    Rd = table.Column<double>(nullable: false),
                    Vol = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LadderStanding", x => new { x.LadderId, x.UserId });
                    table.ForeignKey(
                        name: "FK_LadderStanding_Ladders_LadderId",
                        column: x => x.LadderId,
                        principalTable: "Ladders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LadderStanding_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Message",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Folder = table.Column<int>(nullable: false),
                    FromId = table.Column<string>(nullable: true),
                    IsRead = table.Column<bool>(nullable: false),
                    LastModifiedAt = table.Column<DateTime>(nullable: false),
                    OwnerId = table.Column<string>(nullable: false),
                    RecipientId = table.Column<string>(nullable: true),
                    Subject = table.Column<string>(nullable: true),
                    Text = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Message_AspNetUsers_FromId",
                        column: x => x.FromId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Message_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Message_AspNetUsers_RecipientId",
                        column: x => x.RecipientId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NewsEntries",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    LastModifiedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NewsEntries_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NewsContent",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Language = table.Column<string>(nullable: true),
                    NewsEntryId = table.Column<long>(nullable: false),
                    Text = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsContent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NewsContent_NewsEntries_NewsEntryId",
                        column: x => x.NewsEntryId,
                        principalTable: "NewsEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Alliance",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ChannelId = table.Column<Guid>(nullable: false),
                    ChannelId1 = table.Column<Guid>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alliance", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChatMessages",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ChannelId = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    LastModifiedAt = table.Column<DateTime>(nullable: false),
                    Text = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatMessages_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Channels",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AllianceId = table.Column<long>(nullable: true),
                    CreatedById = table.Column<string>(nullable: true),
                    GameId = table.Column<long>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Channels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Channels_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GameChatMessage",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateTime = table.Column<DateTime>(nullable: false),
                    GameId = table.Column<long>(nullable: false),
                    TeamId = table.Column<Guid>(nullable: true),
                    Text = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameChatMessage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameChatMessage_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HistoryEntry",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Action = table.Column<int>(nullable: false),
                    ActorId = table.Column<Guid>(nullable: true),
                    DateTime = table.Column<DateTime>(nullable: false),
                    DestinationIdentifier = table.Column<string>(nullable: true),
                    GameId = table.Column<long>(nullable: false),
                    OriginIdentifier = table.Column<string>(nullable: true),
                    OtherPlayerId = table.Column<Guid>(nullable: true),
                    Result = table.Column<bool>(nullable: true),
                    TurnNo = table.Column<long>(nullable: false),
                    Units = table.Column<int>(nullable: true),
                    UnitsLost = table.Column<int>(nullable: true),
                    UnitsLostOther = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoryEntry", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Team",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    GameId = table.Column<long>(nullable: false),
                    PlayOrder = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Team", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Player",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Bonus = table.Column<int>(nullable: false),
                    InternalCardData = table.Column<string>(nullable: true),
                    IsHidden = table.Column<bool>(nullable: false),
                    Outcome = table.Column<int>(nullable: false),
                    PlacedInitialUnits = table.Column<bool>(nullable: false),
                    PlayOrder = table.Column<int>(nullable: false),
                    State = table.Column<int>(nullable: false),
                    TeamId = table.Column<Guid>(nullable: false),
                    Timeouts = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Player", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Player_Team_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Team",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Player_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AttacksInCurrentTurn = table.Column<int>(nullable: false),
                    CardDistributed = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    CurrentPlayerId = table.Column<Guid>(nullable: true),
                    LadderId = table.Column<Guid>(nullable: true),
                    LastModifiedAt = table.Column<DateTime>(nullable: false),
                    MapTemplateName = table.Column<string>(nullable: true),
                    MovesInCurrentTurn = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    OptionsId = table.Column<long>(nullable: false),
                    PlayState = table.Column<int>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    SerializedCountries = table.Column<string>(nullable: true),
                    StartedAt = table.Column<DateTime>(nullable: true),
                    State = table.Column<int>(nullable: false),
                    TournamentPairingId = table.Column<Guid>(nullable: true),
                    TurnCounter = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Games_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Games_Ladders_LadderId",
                        column: x => x.LadderId,
                        principalTable: "Ladders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Games_GameOptions_OptionsId",
                        column: x => x.OptionsId,
                        principalTable: "GameOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tournament",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    EndOfTournament = table.Column<DateTime>(nullable: true),
                    LastModifiedAt = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    NumberOfFinalGames = table.Column<int>(nullable: false),
                    NumberOfGroupGames = table.Column<int>(nullable: false),
                    NumberOfKnockoutGames = table.Column<int>(nullable: false),
                    NumberOfTeams = table.Column<int>(nullable: false),
                    OptionsId = table.Column<long>(nullable: false),
                    Phase = table.Column<int>(nullable: false),
                    SerializedMapTemplates = table.Column<string>(nullable: true),
                    StartOfRegistration = table.Column<DateTime>(nullable: false),
                    StartOfTournament = table.Column<DateTime>(nullable: false),
                    State = table.Column<int>(nullable: false),
                    WinnerId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tournament", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tournament_GameOptions_OptionsId",
                        column: x => x.OptionsId,
                        principalTable: "GameOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TournamentGroup",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Number = table.Column<int>(nullable: false),
                    TournamentId = table.Column<Guid>(nullable: false),
                    TournamentId1 = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TournamentGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TournamentGroup_Tournament_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "Tournament",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TournamentGroup_Tournament_TournamentId1",
                        column: x => x.TournamentId1,
                        principalTable: "Tournament",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TournamentTeam",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    GroupId = table.Column<Guid>(nullable: true),
                    GroupOrder = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    State = table.Column<int>(nullable: false),
                    TournamentGroupId = table.Column<Guid>(nullable: true),
                    TournamentId = table.Column<Guid>(nullable: false),
                    TournamentId1 = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TournamentTeam", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TournamentTeam_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TournamentTeam_TournamentGroup_GroupId",
                        column: x => x.GroupId,
                        principalTable: "TournamentGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TournamentTeam_TournamentGroup_TournamentGroupId",
                        column: x => x.TournamentGroupId,
                        principalTable: "TournamentGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TournamentTeam_Tournament_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "Tournament",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TournamentTeam_Tournament_TournamentId1",
                        column: x => x.TournamentId1,
                        principalTable: "Tournament",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TournamentPairing",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    GroupId = table.Column<Guid>(nullable: true),
                    NumberOfGames = table.Column<int>(nullable: false),
                    Order = table.Column<int>(nullable: false),
                    Phase = table.Column<int>(nullable: false),
                    State = table.Column<int>(nullable: false),
                    TeamAId = table.Column<Guid>(nullable: false),
                    TeamAWon = table.Column<int>(nullable: false),
                    TeamBId = table.Column<Guid>(nullable: false),
                    TeamBWon = table.Column<int>(nullable: false),
                    TournamentGroupId = table.Column<Guid>(nullable: true),
                    TournamentId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TournamentPairing", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TournamentPairing_TournamentGroup_GroupId",
                        column: x => x.GroupId,
                        principalTable: "TournamentGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TournamentPairing_TournamentTeam_TeamAId",
                        column: x => x.TeamAId,
                        principalTable: "TournamentTeam",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TournamentPairing_TournamentTeam_TeamBId",
                        column: x => x.TeamBId,
                        principalTable: "TournamentTeam",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TournamentPairing_TournamentGroup_TournamentGroupId",
                        column: x => x.TournamentGroupId,
                        principalTable: "TournamentGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TournamentPairing_Tournament_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "Tournament",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TournamentParticipant",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TeamId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TournamentParticipant", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TournamentParticipant_TournamentTeam_TeamId",
                        column: x => x.TeamId,
                        principalTable: "TournamentTeam",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TournamentParticipant_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Alliance_ChannelId1",
                table: "Alliance",
                column: "ChannelId1",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Channels_CreatedById",
                table: "Channels",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Channels_GameId",
                table: "Channels",
                column: "GameId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_ChannelId",
                table: "ChatMessages",
                column: "ChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_CreatedById",
                table: "ChatMessages",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_GameChatMessage_GameId",
                table: "GameChatMessage",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_GameChatMessage_TeamId",
                table: "GameChatMessage",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_GameChatMessage_UserId",
                table: "GameChatMessage",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_CreatedById",
                table: "Games",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Games_LadderId",
                table: "Games",
                column: "LadderId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_OptionsId",
                table: "Games",
                column: "OptionsId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_TournamentPairingId",
                table: "Games",
                column: "TournamentPairingId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoryEntry_ActorId",
                table: "HistoryEntry",
                column: "ActorId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoryEntry_GameId",
                table: "HistoryEntry",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoryEntry_OtherPlayerId",
                table: "HistoryEntry",
                column: "OtherPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Player_TeamId",
                table: "Player",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Player_UserId",
                table: "Player",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Team_GameId",
                table: "Team",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Ladders_OptionsId",
                table: "Ladders",
                column: "OptionsId");

            migrationBuilder.CreateIndex(
                name: "IX_LadderQueueEntry_UserId",
                table: "LadderQueueEntry",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_LadderStanding_UserId",
                table: "LadderStanding",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_FromId",
                table: "Message",
                column: "FromId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_OwnerId",
                table: "Message",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_RecipientId",
                table: "Message",
                column: "RecipientId");

            migrationBuilder.CreateIndex(
                name: "IX_NewsContent_NewsEntryId",
                table: "NewsContent",
                column: "NewsEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_NewsEntries_CreatedById",
                table: "NewsEntries",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Tournament_OptionsId",
                table: "Tournament",
                column: "OptionsId");

            migrationBuilder.CreateIndex(
                name: "IX_Tournament_WinnerId",
                table: "Tournament",
                column: "WinnerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TournamentGroup_TournamentId",
                table: "TournamentGroup",
                column: "TournamentId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentGroup_TournamentId1",
                table: "TournamentGroup",
                column: "TournamentId1");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentPairing_GroupId",
                table: "TournamentPairing",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentPairing_TeamAId",
                table: "TournamentPairing",
                column: "TeamAId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentPairing_TeamBId",
                table: "TournamentPairing",
                column: "TeamBId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentPairing_TournamentGroupId",
                table: "TournamentPairing",
                column: "TournamentGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentPairing_TournamentId",
                table: "TournamentPairing",
                column: "TournamentId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentParticipant_TeamId",
                table: "TournamentParticipant",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentParticipant_UserId",
                table: "TournamentParticipant",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentTeam_CreatedById",
                table: "TournamentTeam",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentTeam_GroupId",
                table: "TournamentTeam",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentTeam_TournamentGroupId",
                table: "TournamentTeam",
                column: "TournamentGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentTeam_TournamentId",
                table: "TournamentTeam",
                column: "TournamentId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentTeam_TournamentId1",
                table: "TournamentTeam",
                column: "TournamentId1");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_AllianceId",
                table: "AspNetUsers",
                column: "AllianceId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenIddictApplications_ClientId",
                table: "OpenIddictApplications",
                column: "ClientId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OpenIddictAuthorizations_ApplicationId",
                table: "OpenIddictAuthorizations",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenIddictTokens_ApplicationId",
                table: "OpenIddictTokens",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenIddictTokens_AuthorizationId",
                table: "OpenIddictTokens",
                column: "AuthorizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Alliance_AllianceId",
                table: "AspNetUsers",
                column: "AllianceId",
                principalTable: "Alliance",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Alliance_Channels_ChannelId1",
                table: "Alliance",
                column: "ChannelId1",
                principalTable: "Channels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMessages_Channels_ChannelId",
                table: "ChatMessages",
                column: "ChannelId",
                principalTable: "Channels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Channels_Games_GameId",
                table: "Channels",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GameChatMessage_Games_GameId",
                table: "GameChatMessage",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameChatMessage_Team_TeamId",
                table: "GameChatMessage",
                column: "TeamId",
                principalTable: "Team",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HistoryEntry_Games_GameId",
                table: "HistoryEntry",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HistoryEntry_Player_ActorId",
                table: "HistoryEntry",
                column: "ActorId",
                principalTable: "Player",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HistoryEntry_Player_OtherPlayerId",
                table: "HistoryEntry",
                column: "OtherPlayerId",
                principalTable: "Player",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Team_Games_GameId",
                table: "Team",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_TournamentPairing_TournamentPairingId",
                table: "Games",
                column: "TournamentPairingId",
                principalTable: "TournamentPairing",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tournament_TournamentTeam_WinnerId",
                table: "Tournament",
                column: "WinnerId",
                principalTable: "TournamentTeam",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alliance_Channels_ChannelId1",
                table: "Alliance");

            migrationBuilder.DropForeignKey(
                name: "FK_TournamentTeam_AspNetUsers_CreatedById",
                table: "TournamentTeam");

            migrationBuilder.DropForeignKey(
                name: "FK_Tournament_GameOptions_OptionsId",
                table: "Tournament");

            migrationBuilder.DropForeignKey(
                name: "FK_Tournament_TournamentTeam_WinnerId",
                table: "Tournament");

            migrationBuilder.DropTable(
                name: "ChatMessages");

            migrationBuilder.DropTable(
                name: "GameChatMessage");

            migrationBuilder.DropTable(
                name: "HistoryEntry");

            migrationBuilder.DropTable(
                name: "LadderQueueEntry");

            migrationBuilder.DropTable(
                name: "LadderStanding");

            migrationBuilder.DropTable(
                name: "MapTemplates");

            migrationBuilder.DropTable(
                name: "Message");

            migrationBuilder.DropTable(
                name: "NewsContent");

            migrationBuilder.DropTable(
                name: "TournamentParticipant");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "OpenIddictScopes");

            migrationBuilder.DropTable(
                name: "OpenIddictTokens");

            migrationBuilder.DropTable(
                name: "Player");

            migrationBuilder.DropTable(
                name: "NewsEntries");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "OpenIddictAuthorizations");

            migrationBuilder.DropTable(
                name: "Team");

            migrationBuilder.DropTable(
                name: "OpenIddictApplications");

            migrationBuilder.DropTable(
                name: "Channels");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "Ladders");

            migrationBuilder.DropTable(
                name: "TournamentPairing");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Alliance");

            migrationBuilder.DropTable(
                name: "GameOptions");

            migrationBuilder.DropTable(
                name: "TournamentTeam");

            migrationBuilder.DropTable(
                name: "TournamentGroup");

            migrationBuilder.DropTable(
                name: "Tournament");
        }
    }
}
