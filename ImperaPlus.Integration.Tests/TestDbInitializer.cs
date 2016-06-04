using ImperaPlus.DataAccess;
using ImperaPlus.DataAccess.ConvertedMaps;
using ImperaPlus.Domain;
using ImperaPlus.Domain.News;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Linq;

namespace ImperaPlus.Integration.Tests
{
    public class TestDbInitializer : ForceDropDatabaseInitializer
    {
        protected override void Seed(ImperaContext context)
        {
            DbSeed.Seed(context, false);

            context.MapTemplates.Add(Maps.WorldDeluxe());            
            context.MapTemplates.Add(TestMaps.TestMap());

            // Add dummy news entry
            var newsEntry = NewsEntry.Create();
            newsEntry.CreatedBy = context.Users.FirstOrDefault(x => x.UserName == "System");
            newsEntry.CreatedAt = DateTime.UtcNow;
            newsEntry.AddContent("en", "DB initialized", "DB has been updated");
            context.NewsEntries.Add(newsEntry);

            context.SaveChanges();

            // Add admin user
            using (var userManager = new UserManager<User>(new UserStore<User>(context)))
            {
                // Insert technical user
                User testAdminUser = context.Users.FirstOrDefault(x => x.UserName == "TestAdmin");
                if (testAdminUser == null)
                {
                    testAdminUser = new User
                    {
                        UserName = "TestAdmin",
                        GameSlots = int.MaxValue - 1,
                        LockoutEndDateUtc = DateTime.Now,                        
                    };

                    userManager.Create(testAdminUser, "TestAdmin"); // TODO!
                }
            
                userManager.AddToRole(testAdminUser.Id, "admin");
            
                context.SaveChanges();
            }

            base.Seed(context);
        }
    }
}