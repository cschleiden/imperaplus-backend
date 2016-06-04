namespace ImperaPlus.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMessages2 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Messages", new[] { "From_Id" });
            DropIndex("dbo.Messages", new[] { "Recipient_Id" });
            DropColumn("dbo.Messages", "FromId");
            DropColumn("dbo.Messages", "RecipientId");
            RenameColumn(table: "dbo.Messages", name: "From_Id", newName: "FromId");
            RenameColumn(table: "dbo.Messages", name: "Recipient_Id", newName: "RecipientId");
            AlterColumn("dbo.Messages", "FromId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Messages", "RecipientId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Messages", "FromId");
            CreateIndex("dbo.Messages", "RecipientId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Messages", new[] { "RecipientId" });
            DropIndex("dbo.Messages", new[] { "FromId" });
            AlterColumn("dbo.Messages", "RecipientId", c => c.String());
            AlterColumn("dbo.Messages", "FromId", c => c.String());
            RenameColumn(table: "dbo.Messages", name: "RecipientId", newName: "Recipient_Id");
            RenameColumn(table: "dbo.Messages", name: "FromId", newName: "From_Id");
            AddColumn("dbo.Messages", "RecipientId", c => c.String());
            AddColumn("dbo.Messages", "FromId", c => c.String());
            CreateIndex("dbo.Messages", "Recipient_Id");
            CreateIndex("dbo.Messages", "From_Id");
        }
    }
}
