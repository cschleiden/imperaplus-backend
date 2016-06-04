namespace ImperaPlus.DataAccess.Migrations
{
    using System.Data.Entity.Migrations;

    public sealed class Configuration : DbMigrationsConfiguration<ImperaPlus.DataAccess.ImperaContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ImperaPlus.DataAccess.ImperaContext context)
        {
            //  This method will be called after migrating to the latest version.
            DbSeed.Seed(context);
        }
    }
}
