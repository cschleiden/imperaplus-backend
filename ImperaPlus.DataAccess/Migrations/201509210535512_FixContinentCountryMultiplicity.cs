namespace ImperaPlus.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixContinentCountryMultiplicity : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CountryTemplates", "Continent_Id", "dbo.Continents");
            DropIndex("dbo.CountryTemplates", new[] { "Continent_Id" });
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
            
            DropColumn("dbo.CountryTemplates", "Continent_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CountryTemplates", "Continent_Id", c => c.Long());
            DropForeignKey("dbo.ContinentCountryTemplates", "CountryTemplate_Id", "dbo.CountryTemplates");
            DropForeignKey("dbo.ContinentCountryTemplates", "Continent_Id", "dbo.Continents");
            DropIndex("dbo.ContinentCountryTemplates", new[] { "CountryTemplate_Id" });
            DropIndex("dbo.ContinentCountryTemplates", new[] { "Continent_Id" });
            DropTable("dbo.ContinentCountryTemplates");
            CreateIndex("dbo.CountryTemplates", "Continent_Id");
            AddForeignKey("dbo.CountryTemplates", "Continent_Id", "dbo.Continents", "Id");
        }
    }
}
