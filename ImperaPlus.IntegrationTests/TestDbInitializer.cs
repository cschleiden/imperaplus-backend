using System;
using System.Linq;
using System.Threading.Tasks;
using ImperaPlus.DataAccess;
using ImperaPlus.Domain;
using ImperaPlus.Domain.News;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ImperaPlus.Integration.Tests
{
    public class TestDbInitializer
    {
        private RoleManager<IdentityRole> roleManager;
        private UserManager<User> userManager;

        public TestDbInitializer(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        protected async Task Seed(ImperaContext context)
        {
            await new DbSeed(this.userManager, this.roleManager).Seed(context);

            if (context.MapTemplates.FirstOrDefault(x => x.Name == "TestMap") == null)
            {
                context.MapTemplates.Add(new Domain.Map.MapTemplateDescriptor
                {
                    Name = "TestMap",
                    LastModifiedAt = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow
                });
            }

            // Add dummy news entry
            var newsEntry = NewsEntry.Create();
            newsEntry.CreatedBy = context.Users.FirstOrDefault(x => x.UserName == "System");
            newsEntry.CreatedAt = DateTime.UtcNow;
            newsEntry.AddContent("en", "DB initialized", "DB has been updated");
            context.NewsEntries.Add(newsEntry);

            context.SaveChanges();

            // Add admin user

            // Insert technical user
            User testAdminUser = context.Users.FirstOrDefault(x => x.UserName == "TestAdmin");
            if (testAdminUser == null)
            {
                testAdminUser = new User
                {
                    UserName = "TestAdmin",
                    GameSlots = int.MaxValue - 1,
                    LockoutEnabled = false
                };

                await this.userManager.CreateAsync(testAdminUser, "TestAdmin");
            }

            await this.userManager.AddToRoleAsync(testAdminUser, "admin");

            context.SaveChanges();
        }
    }
}