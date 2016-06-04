namespace ImperaPlus.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NullableTournamentEndDate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Tournaments", "EndOfTournament", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Tournaments", "EndOfTournament", c => c.DateTime(nullable: false));
        }
    }
}
