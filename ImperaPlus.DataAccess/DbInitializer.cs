using System;
using System.Linq;
using System.Threading.Tasks;
using ImperaPlus.DataAccess.ConvertedMaps;
using ImperaPlus.Domain;
using ImperaPlus.Domain.Chat;
using ImperaPlus.Domain.Enums;
using ImperaPlus.Domain.Ladders;
using ImperaPlus.Domain.Map;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ImperaPlus.DataAccess
{
    public class DbSeed
    {
        private UserManager<User> userManager;

        public DbSeed(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        public async Task Seed(ImperaContext context)
        {
            // Enable if seed should be debugged locally
            // if (System.Diagnostics.Debugger.IsAttached == false)
            //     System.Diagnostics.Debugger.Launch();

            // Insert roles
            var systemRole = new IdentityRole("system");
            if (context.Roles.FirstOrDefault(x => x.Name == systemRole.Name) == null)
            {
                context.Roles.Add(systemRole);
            }

            var adminRole = new IdentityRole("admin");
            if (context.Roles.FirstOrDefault(x => x.Name == adminRole.Name) == null)
            {
                context.Roles.Add(adminRole);
            }


            // Insert technical user
            User systemUser = context.Users.FirstOrDefault(x => x.UserName == "System");
            if (systemUser == null)
            {
                systemUser = new User
                {
                    UserName = "System",
                    GameSlots = int.MaxValue
                };
                await this.userManager.CreateAsync(systemUser, Guid.NewGuid().ToString());
            }

            await userManager.AddToRolesAsync(systemUser, new[] { "admin", "system" });

            // Insert bot user
            User botUser = context.Users.FirstOrDefault(x => x.UserName == "Bot");
            if (botUser == null)
            {
                botUser = new User
                {
                    UserName = "Bot"
                };
                await this.userManager.CreateAsync(botUser);
            }

            await this.userManager.AddToRoleAsync(botUser, "system");

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
                await this.userManager.CreateAsync(testUser, "impera1234");
            }

            await this.userManager.AddToRoleAsync(testUser, "admin");

            User testUser2 = context.Users.FirstOrDefault(x => x.UserName == "ddtest");
            if (testUser2 == null)
            {
                testUser2 = new User
                {
                    UserName = "ddtest",
                    EmailConfirmed = true
                };
                await this.userManager.CreateAsync(testUser2, "impera1234");
            }
#endif

            context.SaveChanges();

            this.InitChannels(context, systemUser);

            this.InitLadder(context, systemUser);

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

                if (context.MapTemplates.FirstOrDefault(mt => mt.Name == mapTemplateDescriptor.Name) == null)
                {
                    context.MapTemplates.Add(mapTemplateDescriptor);
                }
            }

            context.SaveChanges();
        }

        private void InitChannels(ImperaContext context, User systemUser)
        {
            // Insert default chat channels
            if (context.Channels.FirstOrDefault(c => c.Name == "General") == null)
            {
                context.Channels.Add(new Channel
                {
                    Name = "General",
                    Type = ChannelType.General,
                    CreatedBy = systemUser
                });

                context.Channels.Add(new Channel
                {
                    Name = "Admin",
                    Type = ChannelType.Admin,
                    CreatedBy = systemUser
                });
            }

            context.SaveChanges();
        }

        private void InitLadder(ImperaContext context, User systemUser)
        {
            if (context.Ladders.FirstOrDefault(l => l.Name == "Default") == null)
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

                context.Ladders.Add(ladder);

                context.SaveChanges();
            }
        }
    }
}