using System.Data.Entity;
using ImperaPlus.Domain;
using ImperaPlus.Domain.Chat;
using ImperaPlus.Domain.Games;
using ImperaPlus.Domain.Map;
using ImperaPlus.Domain.News;
using Microsoft.AspNet.Identity.EntityFramework;
using ImperaPlus.Domain.Ladders;

namespace ImperaPlus.DataAccess
{
    public interface IImperaContext
    {
        IDbSet<Game> Games { get; set; }
        
        IDbSet<MapTemplateDescriptor> MapTemplates { get; set; }
        
        IDbSet<Channel> Channels { get; set; }
            
        IDbSet<ChatMessage> ChatMessages { get; set; }

        IDbSet<NewsEntry> NewsEntries { get; set; }
        
        IDbSet<Ladder> Ladders { get; set; }

        /// <remarks>
        /// Required by EF ASP.NET Identity
        /// </remarks>
        IDbSet<User> Users { get; set; }
        
        /// <remarks>
        /// Required by EF ASP.NET Identity
        /// </remarks>
        IDbSet<IdentityRole> Roles { get; set; }
        
        int SaveChanges();
    }
}