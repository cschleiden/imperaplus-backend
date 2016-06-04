namespace ImperaPlus.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialV96 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Channels",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        Type = c.Int(nullable: false),
                        CreatedById = c.String(maxLength: 128),
                        GameId = c.Long(),
                        AllianceId = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedById)
                .ForeignKey("dbo.Games", t => t.GameId)
                .Index(t => t.CreatedById)
                .Index(t => t.GameId);
            
            CreateTable(
                "dbo.Alliances",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        ChannelId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Channels", t => t.Id, cascadeDelete: true)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        AllianceId = c.Guid(),
                        IsAllianceAdmin = c.Boolean(nullable: false),
                        GameSlots = c.Int(nullable: false),
                        Language = c.String(),
                        LegacyPasswordHash = c.String(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Alliances", t => t.AllianceId)
                .Index(t => t.AllianceId)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Games",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        CreatedAt = c.DateTime(nullable: false),
                        CreatedById = c.String(maxLength: 128),
                        LastModifiedAt = c.DateTime(nullable: false),
                        StartedAt = c.DateTime(),
                        LadderId = c.Guid(),
                        TournamentPairingId = c.Guid(),
                        Type = c.Int(nullable: false),
                        Name = c.String(),
                        CountriesJson = c.String(),
                        MapTemplateName = c.String(),
                        State = c.Int(nullable: false),
                        PlayState = c.Int(nullable: false),
                        CurrentPlayerId = c.Guid(),
                        OptionsId = c.Long(nullable: false),
                        TurnCounter = c.Int(nullable: false),
                        AttacksInCurrentTurn = c.Int(nullable: false),
                        MovesInCurrentTurn = c.Int(nullable: false),
                        CardDistributed = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedById)
                .ForeignKey("dbo.Ladders", t => t.LadderId)
                .ForeignKey("dbo.GameOptions", t => t.OptionsId, cascadeDelete: true)
                .ForeignKey("dbo.TournamentPairings", t => t.TournamentPairingId)
                .Index(t => t.CreatedById)
                .Index(t => t.LadderId)
                .Index(t => t.TournamentPairingId)
                .Index(t => t.OptionsId);
            
            CreateTable(
                "dbo.GameChatMessages",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        GameId = c.Long(nullable: false),
                        UserId = c.String(maxLength: 128),
                        TeamId = c.Guid(),
                        DateTime = c.DateTime(nullable: false),
                        Text = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Teams", t => t.TeamId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.Games", t => t.GameId, cascadeDelete: true)
                .Index(t => t.GameId)
                .Index(t => t.UserId)
                .Index(t => t.TeamId);
            
            CreateTable(
                "dbo.Teams",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        GameId = c.Long(nullable: false),
                        PlayOrder = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Games", t => t.GameId, cascadeDelete: true)
                .Index(t => t.GameId);
            
            CreateTable(
                "dbo.Players",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UserId = c.String(maxLength: 128),
                        TeamId = c.Guid(nullable: false),
                        PlayOrder = c.Int(nullable: false),
                        Timeouts = c.Int(nullable: false),
                        IsHidden = c.Boolean(nullable: false),
                        PlacedInitialUnits = c.Boolean(nullable: false),
                        Bonus = c.Int(nullable: false),
                        InternalCardData = c.String(),
                        State = c.Int(nullable: false),
                        Outcome = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.Teams", t => t.TeamId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.TeamId);
            
            CreateTable(
                "dbo.HistoryEntries",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TurnNo = c.Long(nullable: false),
                        DateTime = c.DateTime(nullable: false),
                        GameId = c.Long(nullable: false),
                        ActorId = c.Guid(),
                        OtherPlayerId = c.Guid(),
                        Action = c.Int(nullable: false),
                        OriginIdentifier = c.String(),
                        DestinationIdentifier = c.String(),
                        Units = c.Int(),
                        UnitsLost = c.Int(),
                        UnitsLostOther = c.Int(),
                        Result = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Players", t => t.ActorId)
                .ForeignKey("dbo.Games", t => t.GameId, cascadeDelete: true)
                .ForeignKey("dbo.Players", t => t.OtherPlayerId)
                .Index(t => t.GameId)
                .Index(t => t.ActorId)
                .Index(t => t.OtherPlayerId);
            
            CreateTable(
                "dbo.Ladders",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        MapTemplates = c.String(),
                        Options_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GameOptions", t => t.Options_Id)
                .Index(t => t.Options_Id);
            
            CreateTable(
                "dbo.GameOptions",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        NumberOfPlayersPerTeam = c.Int(nullable: false),
                        NumberOfTeams = c.Int(nullable: false),
                        MinUnitsPerCountry = c.Int(nullable: false),
                        NewUnitsPerTurn = c.Int(nullable: false),
                        AttacksPerTurn = c.Int(nullable: false),
                        MovesPerTurn = c.Int(nullable: false),
                        InitialCountryUnits = c.Int(nullable: false),
                        MapDistribution = c.Int(nullable: false),
                        VictoryConditions = c.String(),
                        VisibilityModifier = c.String(),
                        MaximumNumberOfCards = c.Int(nullable: false),
                        MaximumTimeoutsPerPlayer = c.Int(nullable: false),
                        TimeoutInSeconds = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LadderQueueEntries",
                c => new
                    {
                        LadderId = c.Guid(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                        CreatedAt = c.DateTime(nullable: false),
                        LastModifiedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.LadderId, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Ladders", t => t.LadderId, cascadeDelete: true)
                .Index(t => t.LadderId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.LadderStandings",
                c => new
                    {
                        LadderId = c.Guid(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                        GamesPlayed = c.Int(nullable: false),
                        GamesWon = c.Int(nullable: false),
                        GamesLost = c.Int(nullable: false),
                        LastGame = c.DateTime(nullable: false),
                        Rating = c.Double(nullable: false),
                        Vol = c.Double(nullable: false),
                        Rd = c.Double(nullable: false),
                    })
                .PrimaryKey(t => new { t.LadderId, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Ladders", t => t.LadderId, cascadeDelete: true)
                .Index(t => t.LadderId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.ChatMessages",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Text = c.String(),
                        ChannelId = c.Guid(nullable: false),
                        CreatedById = c.String(maxLength: 128),
                        CreatedAt = c.DateTime(nullable: false),
                        LastModifiedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedById)
                .ForeignKey("dbo.Channels", t => t.ChannelId, cascadeDelete: true)
                .Index(t => t.ChannelId)
                .Index(t => t.CreatedById);
            
            CreateTable(
                "dbo.MapTemplates",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        LastModifiedAt = c.DateTime(nullable: false),
                        Image = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Connections",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Origin = c.String(),
                        Destination = c.String(),
                        MapTemplate_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MapTemplates", t => t.MapTemplate_Id)
                .Index(t => t.MapTemplate_Id);
            
            CreateTable(
                "dbo.Continents",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Bonus = c.Int(nullable: false),
                        MapTemplate_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MapTemplates", t => t.MapTemplate_Id)
                .Index(t => t.MapTemplate_Id);
            
            CreateTable(
                "dbo.CountryTemplates",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Identifier = c.String(),
                        Name = c.String(),
                        X = c.Int(nullable: false),
                        Y = c.Int(nullable: false),
                        MapTemplate_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MapTemplates", t => t.MapTemplate_Id)
                .Index(t => t.MapTemplate_Id);
            
            CreateTable(
                "dbo.NewsEntries",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CreatedById = c.String(maxLength: 128),
                        CreatedAt = c.DateTime(nullable: false),
                        LastModifiedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedById)
                .Index(t => t.CreatedById);
            
            CreateTable(
                "dbo.NewsContents",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Language = c.String(),
                        Title = c.String(),
                        Text = c.String(),
                        NewsEntry_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NewsEntries", t => t.NewsEntry_Id, cascadeDelete: true)
                .Index(t => t.NewsEntry_Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.Tournaments",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        NumberOfTeams = c.Int(nullable: false),
                        OptionsId = c.Long(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        LastModifiedAt = c.DateTime(nullable: false),
                        NumberOfGroupGames = c.Int(nullable: false),
                        NumberOfKnockoutGames = c.Int(nullable: false),
                        NumberOfFinalGames = c.Int(nullable: false),
                        StartOfRegistration = c.DateTime(nullable: false),
                        StartOfTournament = c.DateTime(nullable: false),
                        EndOfTournament = c.DateTime(),
                        Phase = c.Int(nullable: false),
                        State = c.Int(nullable: false),
                        MapTemplates = c.String(),
                        Winner_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GameOptions", t => t.OptionsId, cascadeDelete: true)
                .ForeignKey("dbo.TournamentTeams", t => t.Winner_Id)
                .Index(t => t.OptionsId)
                .Index(t => t.Winner_Id);
            
            CreateTable(
                "dbo.TournamentGroups",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        TournamentId = c.Guid(nullable: false),
                        Number = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tournaments", t => t.TournamentId, cascadeDelete: true)
                .Index(t => t.TournamentId);
            
            CreateTable(
                "dbo.TournamentPairings",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        TournamentId = c.Guid(nullable: false),
                        Phase = c.Int(nullable: false),
                        Order = c.Int(nullable: false),
                        GroupId = c.Guid(),
                        TeamAId = c.Guid(nullable: false),
                        TeamAWon = c.Int(nullable: false),
                        TeamBId = c.Guid(nullable: false),
                        TeamBWon = c.Int(nullable: false),
                        NumberOfGames = c.Int(nullable: false),
                        State = c.Int(nullable: false),
                        TournamentTeam_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TournamentGroups", t => t.GroupId)
                .ForeignKey("dbo.TournamentTeams", t => t.TournamentTeam_Id)
                .ForeignKey("dbo.TournamentTeams", t => t.TeamAId)
                .ForeignKey("dbo.TournamentTeams", t => t.TeamBId)
                .ForeignKey("dbo.Tournaments", t => t.TournamentId, cascadeDelete: true)
                .Index(t => t.TournamentId)
                .Index(t => t.GroupId)
                .Index(t => t.TeamAId)
                .Index(t => t.TeamBId)
                .Index(t => t.TournamentTeam_Id);
            
            CreateTable(
                "dbo.TournamentTeams",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        TournamentId = c.Guid(nullable: false),
                        Name = c.String(),
                        Password = c.String(),
                        GroupOrder = c.Int(nullable: false),
                        GroupId = c.Guid(),
                        State = c.Int(nullable: false),
                        CreatedById = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedById)
                .ForeignKey("dbo.TournamentGroups", t => t.GroupId)
                .ForeignKey("dbo.Tournaments", t => t.TournamentId, cascadeDelete: true)
                .Index(t => t.TournamentId)
                .Index(t => t.GroupId)
                .Index(t => t.CreatedById);
            
            CreateTable(
                "dbo.TournamentParticipants",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        TournamentId = c.Guid(nullable: false),
                        UserId = c.String(maxLength: 128),
                        TeamId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tournaments", t => t.TournamentId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.TournamentTeams", t => t.TeamId)
                .Index(t => t.TournamentId)
                .Index(t => t.UserId)
                .Index(t => t.TeamId);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        LastModifiedAt = c.DateTime(nullable: false),
                        Folder = c.Int(nullable: false),
                        IsRead = c.Boolean(nullable: false),
                        Subject = c.String(),
                        Text = c.String(),
                        OwnerId = c.String(nullable: false, maxLength: 128),
                        FromId = c.String(nullable: false, maxLength: 128),
                        RecipientId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.FromId)
                .ForeignKey("dbo.AspNetUsers", t => t.OwnerId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.RecipientId)
                .Index(t => t.OwnerId)
                .Index(t => t.FromId)
                .Index(t => t.RecipientId);
            
            CreateTable(
                "dbo.ContinentCountryTemplates",
                c => new
                    {
                        Continent_Id = c.Long(nullable: false),
                        CountryTemplate_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.Continent_Id, t.CountryTemplate_Id })
                .ForeignKey("dbo.Continents", t => t.Continent_Id, cascadeDelete: true)
                .ForeignKey("dbo.CountryTemplates", t => t.CountryTemplate_Id, cascadeDelete: true)
                .Index(t => t.Continent_Id)
                .Index(t => t.CountryTemplate_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Messages", "RecipientId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Messages", "OwnerId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Messages", "FromId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Tournaments", "Winner_Id", "dbo.TournamentTeams");
            DropForeignKey("dbo.TournamentTeams", "TournamentId", "dbo.Tournaments");
            DropForeignKey("dbo.TournamentPairings", "TournamentId", "dbo.Tournaments");
            DropForeignKey("dbo.Tournaments", "OptionsId", "dbo.GameOptions");
            DropForeignKey("dbo.TournamentGroups", "TournamentId", "dbo.Tournaments");
            DropForeignKey("dbo.TournamentPairings", "TeamBId", "dbo.TournamentTeams");
            DropForeignKey("dbo.TournamentPairings", "TeamAId", "dbo.TournamentTeams");
            DropForeignKey("dbo.TournamentParticipants", "TeamId", "dbo.TournamentTeams");
            DropForeignKey("dbo.TournamentParticipants", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.TournamentParticipants", "TournamentId", "dbo.Tournaments");
            DropForeignKey("dbo.TournamentPairings", "TournamentTeam_Id", "dbo.TournamentTeams");
            DropForeignKey("dbo.TournamentTeams", "GroupId", "dbo.TournamentGroups");
            DropForeignKey("dbo.TournamentTeams", "CreatedById", "dbo.AspNetUsers");
            DropForeignKey("dbo.TournamentPairings", "GroupId", "dbo.TournamentGroups");
            DropForeignKey("dbo.Games", "TournamentPairingId", "dbo.TournamentPairings");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.NewsEntries", "CreatedById", "dbo.AspNetUsers");
            DropForeignKey("dbo.NewsContents", "NewsEntry_Id", "dbo.NewsEntries");
            DropForeignKey("dbo.CountryTemplates", "MapTemplate_Id", "dbo.MapTemplates");
            DropForeignKey("dbo.Continents", "MapTemplate_Id", "dbo.MapTemplates");
            DropForeignKey("dbo.ContinentCountryTemplates", "CountryTemplate_Id", "dbo.CountryTemplates");
            DropForeignKey("dbo.ContinentCountryTemplates", "Continent_Id", "dbo.Continents");
            DropForeignKey("dbo.Connections", "MapTemplate_Id", "dbo.MapTemplates");
            DropForeignKey("dbo.ChatMessages", "ChannelId", "dbo.Channels");
            DropForeignKey("dbo.ChatMessages", "CreatedById", "dbo.AspNetUsers");
            DropForeignKey("dbo.Channels", "GameId", "dbo.Games");
            DropForeignKey("dbo.Channels", "CreatedById", "dbo.AspNetUsers");
            DropForeignKey("dbo.Alliances", "Id", "dbo.Channels");
            DropForeignKey("dbo.AspNetUsers", "AllianceId", "dbo.Alliances");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Teams", "GameId", "dbo.Games");
            DropForeignKey("dbo.Games", "OptionsId", "dbo.GameOptions");
            DropForeignKey("dbo.LadderStandings", "LadderId", "dbo.Ladders");
            DropForeignKey("dbo.LadderStandings", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.LadderQueueEntries", "LadderId", "dbo.Ladders");
            DropForeignKey("dbo.LadderQueueEntries", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Ladders", "Options_Id", "dbo.GameOptions");
            DropForeignKey("dbo.Games", "LadderId", "dbo.Ladders");
            DropForeignKey("dbo.HistoryEntries", "OtherPlayerId", "dbo.Players");
            DropForeignKey("dbo.HistoryEntries", "GameId", "dbo.Games");
            DropForeignKey("dbo.HistoryEntries", "ActorId", "dbo.Players");
            DropForeignKey("dbo.Games", "CreatedById", "dbo.AspNetUsers");
            DropForeignKey("dbo.GameChatMessages", "GameId", "dbo.Games");
            DropForeignKey("dbo.GameChatMessages", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.GameChatMessages", "TeamId", "dbo.Teams");
            DropForeignKey("dbo.Players", "TeamId", "dbo.Teams");
            DropForeignKey("dbo.Players", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.ContinentCountryTemplates", new[] { "CountryTemplate_Id" });
            DropIndex("dbo.ContinentCountryTemplates", new[] { "Continent_Id" });
            DropIndex("dbo.Messages", new[] { "RecipientId" });
            DropIndex("dbo.Messages", new[] { "FromId" });
            DropIndex("dbo.Messages", new[] { "OwnerId" });
            DropIndex("dbo.TournamentParticipants", new[] { "TeamId" });
            DropIndex("dbo.TournamentParticipants", new[] { "UserId" });
            DropIndex("dbo.TournamentParticipants", new[] { "TournamentId" });
            DropIndex("dbo.TournamentTeams", new[] { "CreatedById" });
            DropIndex("dbo.TournamentTeams", new[] { "GroupId" });
            DropIndex("dbo.TournamentTeams", new[] { "TournamentId" });
            DropIndex("dbo.TournamentPairings", new[] { "TournamentTeam_Id" });
            DropIndex("dbo.TournamentPairings", new[] { "TeamBId" });
            DropIndex("dbo.TournamentPairings", new[] { "TeamAId" });
            DropIndex("dbo.TournamentPairings", new[] { "GroupId" });
            DropIndex("dbo.TournamentPairings", new[] { "TournamentId" });
            DropIndex("dbo.TournamentGroups", new[] { "TournamentId" });
            DropIndex("dbo.Tournaments", new[] { "Winner_Id" });
            DropIndex("dbo.Tournaments", new[] { "OptionsId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.NewsContents", new[] { "NewsEntry_Id" });
            DropIndex("dbo.NewsEntries", new[] { "CreatedById" });
            DropIndex("dbo.CountryTemplates", new[] { "MapTemplate_Id" });
            DropIndex("dbo.Continents", new[] { "MapTemplate_Id" });
            DropIndex("dbo.Connections", new[] { "MapTemplate_Id" });
            DropIndex("dbo.ChatMessages", new[] { "CreatedById" });
            DropIndex("dbo.ChatMessages", new[] { "ChannelId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.LadderStandings", new[] { "UserId" });
            DropIndex("dbo.LadderStandings", new[] { "LadderId" });
            DropIndex("dbo.LadderQueueEntries", new[] { "UserId" });
            DropIndex("dbo.LadderQueueEntries", new[] { "LadderId" });
            DropIndex("dbo.Ladders", new[] { "Options_Id" });
            DropIndex("dbo.HistoryEntries", new[] { "OtherPlayerId" });
            DropIndex("dbo.HistoryEntries", new[] { "ActorId" });
            DropIndex("dbo.HistoryEntries", new[] { "GameId" });
            DropIndex("dbo.Players", new[] { "TeamId" });
            DropIndex("dbo.Players", new[] { "UserId" });
            DropIndex("dbo.Teams", new[] { "GameId" });
            DropIndex("dbo.GameChatMessages", new[] { "TeamId" });
            DropIndex("dbo.GameChatMessages", new[] { "UserId" });
            DropIndex("dbo.GameChatMessages", new[] { "GameId" });
            DropIndex("dbo.Games", new[] { "OptionsId" });
            DropIndex("dbo.Games", new[] { "TournamentPairingId" });
            DropIndex("dbo.Games", new[] { "LadderId" });
            DropIndex("dbo.Games", new[] { "CreatedById" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUsers", new[] { "AllianceId" });
            DropIndex("dbo.Alliances", new[] { "Id" });
            DropIndex("dbo.Channels", new[] { "GameId" });
            DropIndex("dbo.Channels", new[] { "CreatedById" });
            DropTable("dbo.ContinentCountryTemplates");
            DropTable("dbo.Messages");
            DropTable("dbo.TournamentParticipants");
            DropTable("dbo.TournamentTeams");
            DropTable("dbo.TournamentPairings");
            DropTable("dbo.TournamentGroups");
            DropTable("dbo.Tournaments");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.NewsContents");
            DropTable("dbo.NewsEntries");
            DropTable("dbo.CountryTemplates");
            DropTable("dbo.Continents");
            DropTable("dbo.Connections");
            DropTable("dbo.MapTemplates");
            DropTable("dbo.ChatMessages");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.LadderStandings");
            DropTable("dbo.LadderQueueEntries");
            DropTable("dbo.GameOptions");
            DropTable("dbo.Ladders");
            DropTable("dbo.HistoryEntries");
            DropTable("dbo.Players");
            DropTable("dbo.Teams");
            DropTable("dbo.GameChatMessages");
            DropTable("dbo.Games");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Alliances");
            DropTable("dbo.Channels");
        }
    }
}
