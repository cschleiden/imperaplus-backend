using ImperaPlus.Domain;
using ImperaPlus.Domain.Chat;
using ImperaPlus.Domain.Games;
using ImperaPlus.Domain.Ladders;
using ImperaPlus.Domain.Map;
using ImperaPlus.Domain.News;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ImperaPlus.DataAccess
{
    public interface IImperaContext
    {
        DbSet<Game> Games { get; set; }
        
        DbSet<MapTemplateDescriptor> MapTemplates { get; set; }
        
        DbSet<Channel> Channels { get; set; }
            
        DbSet<ChatMessage> ChatMessages { get; set; }

        DbSet<NewsEntry> NewsEntries { get; set; }
        
        DbSet<Ladder> Ladders { get; set; }

        /// <remarks>
        /// Required by EF ASP.NET Identity
        /// </remarks>
        DbSet<User> Users { get; set; }
        
        /// <remarks>
        /// Required by EF ASP.NET Identity
        /// </remarks>
        DbSet<IdentityRole> Roles { get; set; }
        
        int SaveChanges();
    }
}