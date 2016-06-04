namespace ImperaPlus.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewsDelete : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.NewsContents", "NewsEntry_Id", "dbo.NewsEntries");
            DropIndex("dbo.NewsContents", new[] { "NewsEntry_Id" });
            AlterColumn("dbo.NewsContents", "NewsEntry_Id", c => c.Long(nullable: false));
            CreateIndex("dbo.NewsContents", "NewsEntry_Id");
            AddForeignKey("dbo.NewsContents", "NewsEntry_Id", "dbo.NewsEntries", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NewsContents", "NewsEntry_Id", "dbo.NewsEntries");
            DropIndex("dbo.NewsContents", new[] { "NewsEntry_Id" });
            AlterColumn("dbo.NewsContents", "NewsEntry_Id", c => c.Long());
            CreateIndex("dbo.NewsContents", "NewsEntry_Id");
            AddForeignKey("dbo.NewsContents", "NewsEntry_Id", "dbo.NewsEntries", "Id");
        }
    }
}
