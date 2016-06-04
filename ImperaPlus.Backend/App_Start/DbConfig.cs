using System.Data.Entity;
using System.Linq;
using ImperaPlus.DataAccess;
using Autofac;

namespace ImperaPlus.Backend.App_Start
{
    public static class DbConfig
    {
        internal static void Init(IDatabaseInitializer<ImperaContext> dbInitializer)
        {
            if (dbInitializer != null)
            {
                Database.SetInitializer(dbInitializer);

                // Ensure database is explicitly initialized
                using (var scope = DependencyInjectionConfig.Container.BeginLifetimeScope(DependencyInjectionConfig.RequestLifetimeScopeName))
                using (var db = scope.Resolve<ImperaContext>())
                {
                    db.Database.Initialize(true);
                }

                // Apply pending migrations
                var configuration = new ImperaPlus.DataAccess.Migrations.Configuration();
                var migrator = new System.Data.Entity.Migrations.DbMigrator(configuration);
                if (migrator.GetPendingMigrations().Any())
                {
                    migrator.Update();
                }
            }
        }
    }
}