namespace ImperaPlus.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLastGameToStanding : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LadderStandings", "LastGame", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.LadderStandings", "LastGame");
        }
    }
}
