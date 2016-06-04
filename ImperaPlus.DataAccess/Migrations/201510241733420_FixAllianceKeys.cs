namespace ImperaPlus.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixAllianceKeys : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "Alliance_Id", "dbo.Alliances");
            DropForeignKey("dbo.Alliances", "Channel_Id", "dbo.Channels");
            DropForeignKey("dbo.AspNetUsers", "AllianceId", "dbo.Alliances");
            DropForeignKey("dbo.AspNetUsers", "Alliance_Id1", "dbo.Alliances");
            DropIndex("dbo.Alliances", new[] { "Channel_Id" });
            DropIndex("dbo.AspNetUsers", new[] { "AllianceId" });
            DropIndex("dbo.AspNetUsers", new[] { "Alliance_Id" });
            DropIndex("dbo.AspNetUsers", new[] { "Alliance_Id1" });
            DropPrimaryKey("dbo.Alliances");
            DropColumn("dbo.Alliances", "Id");
            DropColumn("dbo.AspNetUsers", "AllianceId");
            RenameColumn(table: "dbo.Alliances", name: "Channel_Id", newName: "Id");
            RenameColumn(table: "dbo.AspNetUsers", name: "Alliance_Id1", newName: "AllianceId");            
            AddColumn("dbo.AspNetUsers", "IsAllianceAdmin", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Alliances", "Id", c => c.Guid(nullable: false));
            DropColumn("dbo.AspNetUsers", "AllianceId");
            AddColumn("dbo.AspNetUsers", "AllianceId", c => c.Guid(nullable: true));
            AddPrimaryKey("dbo.Alliances", "Id");
            CreateIndex("dbo.Alliances", "Id");
            CreateIndex("dbo.AspNetUsers", "AllianceId");
            AddForeignKey("dbo.Alliances", "Id", "dbo.Channels", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUsers", "AllianceId", "dbo.Alliances", "Id");
            DropColumn("dbo.AspNetUsers", "Alliance_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Alliance_Id", c => c.Long());
            DropForeignKey("dbo.AspNetUsers", "AllianceId", "dbo.Alliances");
            DropForeignKey("dbo.Alliances", "Id", "dbo.Channels");
            DropIndex("dbo.AspNetUsers", new[] { "AllianceId" });
            DropIndex("dbo.Alliances", new[] { "Id" });
            DropPrimaryKey("dbo.Alliances");
            AlterColumn("dbo.AspNetUsers", "AllianceId", c => c.Long());
            AlterColumn("dbo.Alliances", "Id", c => c.Long(nullable: false, identity: true));
            DropColumn("dbo.AspNetUsers", "IsAllianceAdmin");
            AddPrimaryKey("dbo.Alliances", "Id");
            RenameColumn(table: "dbo.AspNetUsers", name: "AllianceId", newName: "Alliance_Id1");
            RenameColumn(table: "dbo.Alliances", name: "Id", newName: "Channel_Id");
            AddColumn("dbo.AspNetUsers", "AllianceId", c => c.Long());
            AddColumn("dbo.Alliances", "Id", c => c.Long(nullable: false, identity: true));
            CreateIndex("dbo.AspNetUsers", "Alliance_Id1");
            CreateIndex("dbo.AspNetUsers", "Alliance_Id");
            CreateIndex("dbo.AspNetUsers", "AllianceId");
            CreateIndex("dbo.Alliances", "Channel_Id");
            AddForeignKey("dbo.AspNetUsers", "Alliance_Id1", "dbo.Alliances", "Id");
            AddForeignKey("dbo.AspNetUsers", "AllianceId", "dbo.Alliances", "Id");
            AddForeignKey("dbo.Alliances", "Channel_Id", "dbo.Channels", "Id");
            AddForeignKey("dbo.AspNetUsers", "Alliance_Id", "dbo.Alliances", "Id");
        }
    }
}
