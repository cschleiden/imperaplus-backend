using Microsoft.EntityFrameworkCore;
using System.Linq;
using ImperaPlus.Domain.Map;
using ImperaPlus.Domain.Repositories;

namespace ImperaPlus.DataAccess.Repositories
{
    internal class MapTemplateDescriptorRepository : GenericRepository<MapTemplateDescriptor>,
        IMapTemplateDescriptorRepository
    {
        public MapTemplateDescriptorRepository(DbContext context)
            : base(context)
        {
        }

        public MapTemplateDescriptor Get(string name)
        {
            return
                DbSet
                    .FirstOrDefault(x => x.Name == name);
        }
    }
}
