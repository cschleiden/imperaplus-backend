namespace ImperaPlus.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddMessages : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Messages", "ChannelId", "dbo.Channels");
            DropForeignKey("dbo.Messages", "CreatedById", "dbo.AspNetUsers");
            RenameTable(name: "dbo.Messages", newName: "ChatMessages");
            AddForeignKey("dbo.ChatMessages", "CreatedById", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.ChatMessages", "ChannelId", "dbo.Channels", "Id", cascadeDelete: true);

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
                    FromId = c.String(),
                    RecipientId = c.String(),
                    From_Id = c.String(nullable: false, maxLength: 128),
                    Recipient_Id = c.String(nullable: false, maxLength: 128),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.From_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.OwnerId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.Recipient_Id)
                .Index(t => t.OwnerId)
                .Index(t => t.From_Id)
                .Index(t => t.Recipient_Id);
        }

        public override void Down()
        {
            DropForeignKey("dbo.Messages", "Recipient_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Messages", "OwnerId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Messages", "From_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Messages", new[] { "Recipient_Id" });
            DropIndex("dbo.Messages", new[] { "From_Id" });
            DropIndex("dbo.Messages", new[] { "OwnerId" });
            DropTable("dbo.Messages");


            DropForeignKey("dbo.ChatMessages", "CreatedById", "dbo.AspNetUsers");
            DropForeignKey("dbo.ChatMessages", "ChannelId", "dbo.Channels");
            RenameTable(name: "dbo.ChatMessages", newName: "Messages");
            AddForeignKey("dbo.Messages", "CreatedById", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Messages", "ChannelId", "dbo.Channels", "Id", cascadeDelete: true);
        }
    }
}
