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

namespace ImperaPlus.DataAccess
{
    public class DbSeed
    {
        protected UserManager<User> userManager;
        protected RoleManager<IdentityRole> roleManager;

        public DbSeed(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public virtual async Task Seed(ImperaContext context)
        {
            // Enable if seed should be debugged locally
            // if (System.Diagnostics.Debugger.IsAttached == false)
            //     System.Diagnostics.Debugger.Launch();

            // Insert roles
            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }

            if (await roleManager.FindByNameAsync("system") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("system"));
            }

            // Insert technical user
            var systemUser = await userManager.FindByNameAsync("System");
            if (systemUser == null)
            {
                systemUser = new User
                {
                    UserName = "System",
                    Email = "system@imperaonline.de",
                    EmailConfirmed = true,
                    GameSlots = int.MaxValue
                };
                var result = await userManager.CreateAsync(systemUser, Guid.NewGuid().ToString());
                if (!result.Succeeded)
                {
                    throw new Exception();
                }
            }

            await userManager.AddToRolesAsync(systemUser, new[] { "admin", "system" });

            // Insert bot user
            var botUser = await userManager.FindByNameAsync("Bot");
            if (botUser == null)
            {
                botUser = new User
                {
                    UserName = "Bot", Email = "bot@imperaonline.de", EmailConfirmed = true, GameSlots = int.MaxValue
                };
                await userManager.CreateAsync(botUser, Guid.NewGuid().ToString());
            }

            await userManager.AddToRoleAsync(botUser, "system");

            // Insert ghost user
            var ghostUser = await userManager.FindByNameAsync("Ghost");
            if (ghostUser == null)
            {
                ghostUser = new User
                {
                    UserName = "Ghost",
                    Email = "ghost@imperaonline.de",
                    EmailConfirmed = true,
                    GameSlots = int.MaxValue
                };
                await userManager.CreateAsync(ghostUser, Guid.NewGuid().ToString());
            }

            await userManager.AddToRoleAsync(ghostUser, "system");

#if DEBUG
            // Insert test user
            var testUser = context.Users.FirstOrDefault(x => x.UserName == "digitald");
            if (testUser == null)
            {
                testUser = new User
                {
                    UserName = "digitald", Email = "digitald@imperaonline.de", EmailConfirmed = true
                };
                await userManager.CreateAsync(testUser, "impera1234");
            }

            await userManager.AddToRoleAsync(testUser, "admin");

            var testUser2 = context.Users.FirstOrDefault(x => x.UserName == "ddtest");
            if (testUser2 == null)
            {
                testUser2 = new User { UserName = "ddtest", Email = "ddtest@imperaonline.de", EmailConfirmed = true };
                await userManager.CreateAsync(testUser2, "impera1234");
            }

            // News
            var newsEntry = Domain.News.NewsEntry.Create();
            newsEntry.CreatedBy = newsEntry.CreatedBy = testUser;
            newsEntry.LastModifiedAt = newsEntry.CreatedAt = newsEntry.CreatedAt = DateTime.UtcNow;
            newsEntry.AddContent("en", "Title", "This is a news entry.");
            context.NewsEntries.Add(newsEntry);
#endif

            context.SaveChanges();

            InitChannels(context, systemUser);

            InitLadder(context, systemUser);

            // Default set of maps
            var mapType = typeof(Maps);
            foreach (var mapMethod in mapType.GetMethods(System.Reflection.BindingFlags.Public |
                                                         System.Reflection.BindingFlags.Static))
            {
                var mapName = mapMethod.Name;
                var mapTemplateDescriptor = new MapTemplateDescriptor { Name = mapName, IsActive = true };

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
                context.Channels.Add(new Channel { Name = "General", Type = ChannelType.General });

                context.Channels.Add(new Channel { Name = "Admin", Type = ChannelType.Admin });
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
