
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
    public class TestDbSeed : DbSeed
    {
        public TestDbSeed(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
            : base(userManager, roleManager)
        {
        }

        public override async Task Seed(ImperaContext context)
        {
            await base.Seed(context);

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
        }
    }
}