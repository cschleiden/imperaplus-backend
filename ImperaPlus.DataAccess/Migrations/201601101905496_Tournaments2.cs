namespace ImperaPlus.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tournaments2 : DbMigration
    {
        public override void Up()
        {
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
                        EndOfTournament = c.DateTime(nullable: false),
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
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TournamentGroups", t => t.GroupId)
                .ForeignKey("dbo.TournamentTeams", t => t.Id)
                .ForeignKey("dbo.Tournaments", t => t.TournamentId, cascadeDelete: true)
                .Index(t => t.Id)
                .Index(t => t.TournamentId)
                .Index(t => t.GroupId);
            
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
                .ForeignKey("dbo.TournamentTeams", t => t.TeamId)
                .ForeignKey("dbo.Tournaments", t => t.TournamentId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.TournamentId)
                .Index(t => t.UserId)
                .Index(t => t.TeamId);
            
            AddColumn("dbo.Games", "TournamentId", c => c.Guid());
            AddColumn("dbo.Games", "TournamentPairingId", c => c.Guid());
            CreateIndex("dbo.Games", "TournamentId");
            CreateIndex("dbo.Games", "TournamentPairingId");
            AddForeignKey("dbo.Games", "TournamentId", "dbo.Tournaments", "Id");
            AddForeignKey("dbo.Games", "TournamentPairingId", "dbo.TournamentPairings", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tournaments", "Winner_Id", "dbo.TournamentTeams");
            DropForeignKey("dbo.Tournaments", "OptionsId", "dbo.GameOptions");
            DropForeignKey("dbo.TournamentGroups", "TournamentId", "dbo.Tournaments");
            DropForeignKey("dbo.TournamentPairings", "TournamentId", "dbo.Tournaments");
            DropForeignKey("dbo.TournamentPairings", "Id", "dbo.TournamentTeams");
            DropForeignKey("dbo.TournamentTeams", "TournamentId", "dbo.Tournaments");
            DropForeignKey("dbo.TournamentParticipants", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.TournamentParticipants", "TournamentId", "dbo.Tournaments");
            DropForeignKey("dbo.TournamentParticipants", "TeamId", "dbo.TournamentTeams");
            DropForeignKey("dbo.TournamentTeams", "GroupId", "dbo.TournamentGroups");
            DropForeignKey("dbo.TournamentTeams", "CreatedById", "dbo.AspNetUsers");
            DropForeignKey("dbo.TournamentPairings", "GroupId", "dbo.TournamentGroups");
            DropForeignKey("dbo.Games", "TournamentPairingId", "dbo.TournamentPairings");
            DropForeignKey("dbo.Games", "TournamentId", "dbo.Tournaments");
            DropIndex("dbo.TournamentParticipants", new[] { "TeamId" });
            DropIndex("dbo.TournamentParticipants", new[] { "UserId" });
            DropIndex("dbo.TournamentParticipants", new[] { "TournamentId" });
            DropIndex("dbo.TournamentTeams", new[] { "CreatedById" });
            DropIndex("dbo.TournamentTeams", new[] { "GroupId" });
            DropIndex("dbo.TournamentTeams", new[] { "TournamentId" });
            DropIndex("dbo.TournamentPairings", new[] { "GroupId" });
            DropIndex("dbo.TournamentPairings", new[] { "TournamentId" });
            DropIndex("dbo.TournamentPairings", new[] { "Id" });
            DropIndex("dbo.TournamentGroups", new[] { "TournamentId" });
            DropIndex("dbo.Tournaments", new[] { "Winner_Id" });
            DropIndex("dbo.Tournaments", new[] { "OptionsId" });
            DropIndex("dbo.Games", new[] { "TournamentPairingId" });
            DropIndex("dbo.Games", new[] { "TournamentId" });
            DropColumn("dbo.Games", "TournamentPairingId");
            DropColumn("dbo.Games", "TournamentId");
            DropTable("dbo.TournamentParticipants");
            DropTable("dbo.TournamentTeams");
            DropTable("dbo.TournamentPairings");
            DropTable("dbo.TournamentGroups");
            DropTable("dbo.Tournaments");
        }
    }
}
