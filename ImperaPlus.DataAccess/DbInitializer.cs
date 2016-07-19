using System;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using ImperaPlus.DataAccess.ConvertedMaps;
using ImperaPlus.Domain;
using ImperaPlus.Domain.Chat;
using ImperaPlus.Domain.Enums;
using ImperaPlus.Domain.News;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using ImperaPlus.Domain.Ladders;
using ImperaPlus.Domain.Map;

namespace ImperaPlus.DataAccess
{
    public class DbInitializer
        // : ForceDropDatabaseIfModelChangesInitializer
        //: ForceDropDatabaseInitializer
        : MigrateDatabaseToLatestVersion<ImperaContext, ImperaPlus.DataAccess.Migrations.Configuration>
    {
        public override void InitializeDatabase(ImperaContext context)
        {
            base.InitializeDatabase(context);
        }
    }

    public class ForceDropDatabaseIfModelChangesInitializer
        : IDatabaseInitializer<ImperaContext>
    {
        public virtual void InitializeDatabase(ImperaContext context)
        {
            if (!context.Database.CompatibleWithModel(false))
            {
                if (context.Database.Exists())
                {
                    context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction,
                        "ALTER DATABASE [" + context.Database.Connection.Database +
                        "] SET SINGLE_USER WITH ROLLBACK IMMEDIATE");
                    context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction,
                        "USE master DROP DATABASE [" + context.Database.Connection.Database + "]");
                }

                context.Database.Create();

                this.Seed(context);
            }
        }

        protected virtual void Seed(ImperaContext context) { }
    }

    public class ForceDropDatabaseInitializer
        : IDatabaseInitializer<ImperaContext>
    {
        public virtual void InitializeDatabase(ImperaContext context)
        {
            if (context.Database.Exists())
            {
                context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction,
                    "ALTER DATABASE [" + context.Database.Connection.Database +
                    "] SET SINGLE_USER WITH ROLLBACK IMMEDIATE");
                context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction,
                    "USE master DROP DATABASE [" + context.Database.Connection.Database + "]");
            }

            context.Database.Create();

            this.Seed(context);
        }

        protected virtual void Seed(ImperaContext context) { }
    }

    public static class DbSeed
    {
        public static void Seed(ImperaContext context)
        {
            // Enable if seed should be debugged locally
            // if (System.Diagnostics.Debugger.IsAttached == false)
            //     System.Diagnostics.Debugger.Launch();

            // Insert roles
            var systemRole = new IdentityRole("system");
            context.Roles.AddOrUpdate(x => x.Name, systemRole);

            var adminRole = new IdentityRole("admin");
            context.Roles.AddOrUpdate(x => x.Name, adminRole);

            using (var userManager = new UserManager<User>(new UserStore<User>(context)))
            {
                // Insert technical user
                User systemUser = context.Users.FirstOrDefault(x => x.UserName == "System");
                if (systemUser == null)
                {
                    systemUser = new User
                    {
                        UserName = "System",
                        GameSlots = int.MaxValue
                    };
                    userManager.Create(systemUser, Guid.NewGuid().ToString()); // TODO!
                }

                userManager.AddToRole(systemUser.Id, "admin");
                userManager.AddToRole(systemUser.Id, "system");

                // Insert bot user
                User botUser = context.Users.FirstOrDefault(x => x.UserName == "Bot");
                if (botUser == null)
                {
                    botUser = new User
                    {
                        UserName = "Bot"
                    };
                    userManager.Create(botUser);
                }

                userManager.AddToRole(botUser.Id, "system");

#if DEBUG
                // Insert test user
                User testUser = context.Users.FirstOrDefault(x => x.UserName == "digitald");
                if (testUser == null)
                {
                    testUser = new User
                    {
                        UserName = "digitald",
                        EmailConfirmed = true
                    };
                    userManager.Create(testUser, "impera1234");
                }

                userManager.AddToRole(testUser.Id, "admin");

                User testUser2 = context.Users.FirstOrDefault(x => x.UserName == "ddtest");
                if (testUser2 == null)
                {
                    testUser2 = new User
                    {
                        UserName = "ddtest",
                        EmailConfirmed = true
                    };
                    userManager.Create(testUser2, "impera1234");
                }
#endif

                context.SaveChanges();

                InitChannels(context, systemUser);

                InitLadder(context, systemUser);

                // Default set of maps
                var mapType = typeof(Maps);
                foreach (var mapMethod in mapType.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static))
                {
                    var mapName = mapMethod.Name;
                    var mapTemplateDescriptor = new MapTemplateDescriptor
                    {
                        Name = mapName
                    };

                    mapTemplateDescriptor.LastModifiedAt = mapTemplateDescriptor.CreatedAt = DateTime.UtcNow;
                    context.MapTemplates.AddOrUpdate(x => x.Name, mapTemplateDescriptor);
                }

                context.SaveChanges();
            }
        }

        private static void InitChannels(ImperaContext context, User systemUser)
        {
            // Insert default chat channels
            context.Channels.AddOrUpdate(x => x.Name, new Channel
            {
                Name = "General",
                Type = ChannelType.General,
                CreatedBy = systemUser
            });
            context.Channels.AddOrUpdate(x => x.Name, new Channel
            {
                Name = "Admin",
                Type = ChannelType.Admin,
                CreatedBy = systemUser
            });

            context.SaveChanges();
        }

        private static void InitLadder(ImperaContext context, User systemUser)
        {
            var ladder = new Ladder("Default", 2, 2);

            ladder.MapTemplates.Add("WorldDeluxe");

            ladder.Options.MapDistribution = MapDistribution.Default;
            ladder.Options.VisibilityModifier.Add(VisibilityModifierType.None);
            ladder.Options.VictoryConditions.Add(VictoryConditionType.Survival);

            ladder.Options.AttacksPerTurn = 3;
            ladder.Options.InitialCountryUnits = 3;
            ladder.Options.MapDistribution = MapDistribution.Default;
            ladder.Options.MaximumNumberOfCards = 5;
            ladder.Options.MaximumTimeoutsPerPlayer = 1;
            ladder.Options.MinUnitsPerCountry = 1;
            ladder.Options.MovesPerTurn = 3;
            ladder.Options.NewUnitsPerTurn = 3;
            ladder.Options.TimeoutInSeconds = (int)TimeSpan.FromDays(1).TotalSeconds;

            ladder.ToggleActive(true);

            context.Ladders.AddOrUpdate(x => x.Name, ladder);

            context.SaveChanges();
        }
    }
}