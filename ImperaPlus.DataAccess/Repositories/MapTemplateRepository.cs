using System.Data.Entity;
using System.Linq;
using ImperaPlus.Domain.Map;
using ImperaPlus.Domain.Repositories;

namespace ImperaPlus.DataAccess.Repositories
{
    class MapTemplateDescriptorRepository : GenericRepository<MapTemplateDescriptor>, IMapTemplateDescriptorRepository
    {
        public MapTemplateDescriptorRepository(DbContext context) 
            : base(context)
        {
        }

        public MapTemplateDescriptor Get(string name)
        {
            return
                this.DbSet
                    .FirstOrDefault(x => x.Name == name);
        }
    }
}